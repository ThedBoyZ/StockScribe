using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using StockScribe.Api.Domain;
using StockScribe.Api.Features.Auth;
using StockScribe.Api.Infrastructure.Mongo;
using StockScribe.Api.Infrastructure.Persistence;
using StockScribe.Api.Seed;

namespace StockScribe.Api.Features.PurchaseOrders;

public sealed record SupplierRef(Guid Id, string Name);

public sealed record PoLineResponse(Guid Id, string ProductName, string Unit, int QtyOrdered, decimal UnitPrice, decimal LineTotal);

public sealed record ApprovalResponse(string RequiredRole, decimal Limit, decimal? Remaining, bool CanApprove, string? Message);

public sealed record PurchaseOrderResponse(
    Guid Id,
    string Number,
    string Status,
    uint Version,
    SupplierRef Supplier,
    string? DeliverTo,
    decimal Subtotal,
    decimal VatAmount,
    decimal Total,
    DateTimeOffset? SubmittedAt,
    IReadOnlyList<PoLineResponse> Lines,
    ApprovalResponse Approval);

public sealed record DecisionRequest(uint Version, string? Reason);

public sealed record DecisionResponse(Guid Id, string Number, string Status, string By, DateTimeOffset At, uint Version);

/// <summary>US-002 · approve / reject a PO within the approval limit (Slice 2, docs/sa/api-spec.md).</summary>
public static class PurchaseOrderEndpoints
{
    private const string ProblemTypeBase = "https://stockscribe.dev/errors/";

    public static RouteGroupBuilder MapPurchaseOrderEndpoints(this RouteGroupBuilder api)
    {
        var po = api.MapGroup("/purchase-orders").WithTags("PurchaseOrders");
        po.MapGet("/", List);
        po.MapGet("/{id:guid}", Get);
        po.MapPost("/{id:guid}/approve", Approve);
        po.MapPost("/{id:guid}/reject", Reject);

        api.MapPost("/demo/reset", ResetDemo).WithTags("Demo");
        return api;
    }

    /// <summary>
    /// GET /purchase-orders?status=PendingApproval&amp;mine=true. Branch managers only ever see POs for their branch;
    /// staff get 403 until receiving (Slice 3).
    /// </summary>
    private static async Task<Results<Ok<List<PurchaseOrderResponse>>, ProblemHttpResult>> List(
        ClaimsPrincipal principal, AppDbContext db, string? status, bool? mine, CancellationToken ct)
    {
        var actor = principal.ToActor();
        if (actor.Role is UserRole.Staff or UserRole.Admin)
            return Problem(DomainException.Forbidden("forbidden", "ไม่มีสิทธิ์ดูรายการ PO"));

        var query = Load(db);
        if (actor.Role == UserRole.BranchManager)
            query = query.Where(p => p.ApprovalBranchId == actor.BranchId);
        if (status is not null)
        {
            if (!Enum.TryParse<PurchaseOrderStatus>(status, ignoreCase: true, out var parsed))
                return Problem(DomainException.Invalid("invalid-status", $"ไม่รู้จักสถานะ {status}"));
            query = query.Where(p => p.Status == parsed);
        }
        if (mine == true && actor.Role == UserRole.Owner)
            query = query.Where(p => p.Status == PurchaseOrderStatus.PendingApproval);

        var orders = await query.OrderBy(p => p.Number).ToListAsync(ct);
        return TypedResults.Ok(orders.Select(p => ToResponse(p, actor)).ToList());
    }

    private static async Task<Results<Ok<PurchaseOrderResponse>, NotFound, ProblemHttpResult>> Get(
        Guid id, ClaimsPrincipal principal, AppDbContext db, CancellationToken ct)
    {
        var actor = principal.ToActor();
        var po = await Load(db).SingleOrDefaultAsync(p => p.Id == id, ct);
        if (po is null) return TypedResults.NotFound();
        if (CanView(actor, po) is { } denied) return Problem(denied);
        return TypedResults.Ok(ToResponse(po, actor));
    }

    private static Task<Results<Ok<DecisionResponse>, NotFound, ProblemHttpResult>> Approve(
        Guid id, DecisionRequest request, ClaimsPrincipal principal, AppDbContext db, AuditLog audit,
        TimeProvider time, CancellationToken ct) =>
        Decide(id, request, principal, db, audit, time, "PurchaseOrder.Approved",
            (po, actor, now) => po.Approve(actor, now), ct);

    private static Task<Results<Ok<DecisionResponse>, NotFound, ProblemHttpResult>> Reject(
        Guid id, DecisionRequest request, ClaimsPrincipal principal, AppDbContext db, AuditLog audit,
        TimeProvider time, CancellationToken ct) =>
        Decide(id, request, principal, db, audit, time, "PurchaseOrder.Rejected",
            (po, actor, _) => po.Reject(actor, request.Reason ?? string.Empty), ct);

    private static async Task<Results<Ok<DecisionResponse>, NotFound, ProblemHttpResult>> Decide(
        Guid id, DecisionRequest request, ClaimsPrincipal principal, AppDbContext db, AuditLog audit,
        TimeProvider time, string action, Action<PurchaseOrder, Actor, DateTimeOffset> decide, CancellationToken ct)
    {
        var actor = principal.ToActor();
        var po = await db.PurchaseOrders.SingleOrDefaultAsync(p => p.Id == id, ct);
        if (po is null) return TypedResults.NotFound();
        if (CanView(actor, po) is { } denied) return Problem(denied);
        if (po.Version != request.Version) return Problem(ConcurrencyConflict(po.Number));

        var now = time.GetUtcNow();
        try
        {
            decide(po, actor, now);
            await db.SaveChangesAsync(ct);
        }
        catch (DomainException ex)
        {
            return Problem(ex);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Problem(ConcurrencyConflict(po.Number));
        }

        await audit.WriteAsync(new AuditEntry
        {
            At = now.UtcDateTime,
            Action = action,
            EntityType = nameof(PurchaseOrder),
            EntityId = po.Id,
            EntityNumber = po.Number,
            UserId = actor.UserId,
            UserName = actor.Name,
            UserRole = actor.Role.ToString(),
            Data = new BsonDocument
            {
                { "subtotal", po.Subtotal.ToString("0.00") },
                { "status", po.Status.ToString() },
                { "reason", (BsonValue?)po.RejectReason ?? BsonNull.Value },
            },
        }, ct);

        return TypedResults.Ok(new DecisionResponse(po.Id, po.Number, po.Status.ToString(), actor.Name, now, po.Version));
    }

    /// <summary>POST /demo/reset: puts the demo POs back in the approval queue (seed-data.md "รีเซ็ตข้อมูลเดโม").</summary>
    private static async Task<Results<NoContent, NotFound>> ResetDemo(
        IOptions<DemoOptions> demo, DemoSeeder seeder, CancellationToken ct)
    {
        if (!demo.Value.Enabled) return TypedResults.NotFound();
        await seeder.ResetPurchaseOrdersAsync(ct);
        return TypedResults.NoContent();
    }

    private static IQueryable<PurchaseOrder> Load(AppDbContext db) => db.PurchaseOrders.AsNoTracking()
        .Include(p => p.Supplier)
        .Include(p => p.ApprovalBranch)
        .Include(p => p.Lines).ThenInclude(l => l.Product)
        .AsSplitQuery();

    /// <summary>rbac.md: a PO of another branch is 403 (not 404) for branch roles.</summary>
    private static DomainException? CanView(Actor actor, PurchaseOrder po) => actor.Role switch
    {
        UserRole.Owner or UserRole.Purchasing => null,
        UserRole.BranchManager when po.ApprovalBranchId == actor.BranchId => null,
        _ => DomainException.Forbidden("forbidden", "ไม่มีสิทธิ์ดู PO นี้"),
    };

    private static DomainException ConcurrencyConflict(string number) =>
        new("concurrency-conflict", $"PO {number} ถูกแก้ไขไปแล้ว โหลดข้อมูลใหม่ก่อนทำรายการ", 409);

    private static ProblemHttpResult Problem(DomainException ex) =>
        TypedResults.Problem(title: ex.Title, statusCode: ex.StatusCode, type: ProblemTypeBase + ex.Type);

    private static PurchaseOrderResponse ToResponse(PurchaseOrder po, Actor actor)
    {
        var check = po.CheckApproval(actor);
        return new PurchaseOrderResponse(
            po.Id,
            po.Number,
            po.Status.ToString(),
            po.Version,
            new SupplierRef(po.Supplier.Id, po.Supplier.Name),
            po.ApprovalBranch?.Name,
            po.Subtotal,
            po.VatAmount,
            po.Total,
            po.SubmittedAt,
            po.Lines.Select(l => new PoLineResponse(l.Id, l.Product.Name, l.UnitName, l.QtyOrdered, l.UnitPrice, l.LineTotal)).ToList(),
            new ApprovalResponse(check.RequiredRole.ToString(), check.Limit, check.Remaining, check.CanApprove, check.Message));
    }
}
