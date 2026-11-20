using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StockScribe.Api.Domain;
using StockScribe.Api.Features.Auth;
using StockScribe.Api.Infrastructure.Persistence;

namespace StockScribe.Api.Seed;

/// <summary>
/// Inserts the demo data that is missing (docs/sa/seed-data.md). Safe to run on every start:
/// branches match on code, personas on persona_key, suppliers on name, products on SKU, POs on number.
/// </summary>
public sealed class DemoSeeder(AppDbContext db, TimeProvider time, ILogger<DemoSeeder> logger)
{
    public async Task SeedAsync(CancellationToken ct = default)
    {
        var branches = await SeedBranchesAsync(ct);
        await SeedPersonasAsync(branches, ct);
        var suppliers = await SeedSuppliersAsync(ct);
        var products = await SeedProductsAsync(ct);
        await SeedPurchaseOrdersAsync(branches, suppliers, products, ct);

        var added = await db.SaveChangesAsync(ct);
        logger.LogInformation("Demo seed finished, {Added} rows added", added);
    }

    /// <summary>Deletes the demo POs and seeds them again, so the approval queue is full for the next visitor.</summary>
    public async Task ResetPurchaseOrdersAsync(CancellationToken ct = default)
    {
        var numbers = DemoPurchasing.PendingApproval.Select(p => p.Number).ToList();
        await db.PurchaseOrders.Where(p => numbers.Contains(p.Number)).ExecuteDeleteAsync(ct); // lines cascade
        await SeedAsync(ct);
    }

    private async Task<Dictionary<string, Branch>> SeedBranchesAsync(CancellationToken ct)
    {
        var codes = DemoPersonas.Branches.Select(b => b.Code).ToList();
        var branches = await db.Branches.Where(b => codes.Contains(b.Code)).ToDictionaryAsync(b => b.Code, ct);
        foreach (var (code, name) in DemoPersonas.Branches.Where(b => !branches.ContainsKey(b.Code)))
            db.Branches.Add(branches[code] = new Branch(code, name));
        return branches;
    }

    private async Task SeedPersonasAsync(Dictionary<string, Branch> branches, CancellationToken ct)
    {
        var existing = await db.Users.Where(u => u.PersonaKey != null).Select(u => u.PersonaKey!).ToListAsync(ct);
        foreach (var p in DemoPersonas.All.Where(p => !existing.Contains(p.Key)))
        {
            var branchId = p.BranchCode is null ? (Guid?)null : branches[p.BranchCode].Id;
            db.Users.Add(User.CreatePersona(p.Key, p.Name, p.Role, branchId));
        }
    }

    private async Task<Dictionary<string, Supplier>> SeedSuppliersAsync(CancellationToken ct)
    {
        var names = DemoPurchasing.Suppliers.Select(s => s.Name).ToList();
        var suppliers = await db.Suppliers.Where(s => names.Contains(s.Name)).ToDictionaryAsync(s => s.Name, ct);
        foreach (var s in DemoPurchasing.Suppliers.Where(s => !suppliers.ContainsKey(s.Name)))
            db.Suppliers.Add(suppliers[s.Name] = new Supplier(s.Name, s.MinOrderAmount));
        return suppliers;
    }

    private async Task<Dictionary<string, Product>> SeedProductsAsync(CancellationToken ct)
    {
        var skus = DemoPurchasing.Products.Select(p => p.Sku).ToList();
        var products = await db.Products.Where(p => skus.Contains(p.Sku)).ToDictionaryAsync(p => p.Sku, ct);
        foreach (var p in DemoPurchasing.Products.Where(p => !products.ContainsKey(p.Sku)))
            db.Products.Add(products[p.Sku] = new Product(p.Sku, p.Name));
        return products;
    }

    private async Task SeedPurchaseOrdersAsync(
        Dictionary<string, Branch> branches,
        Dictionary<string, Supplier> suppliers,
        Dictionary<string, Product> products,
        CancellationToken ct)
    {
        var numbers = DemoPurchasing.PendingApproval.Select(p => p.Number).ToList();
        var existing = await db.PurchaseOrders.Where(p => numbers.Contains(p.Number)).Select(p => p.Number).ToListAsync(ct);
        var now = time.GetUtcNow();

        foreach (var (demo, i) in DemoPurchasing.PendingApproval.Select((d, i) => (d, i)).Where(x => !existing.Contains(x.d.Number)))
        {
            var po = new PurchaseOrder(demo.Number, suppliers[demo.Supplier].Id);
            foreach (var line in demo.Lines)
                po.AddLine(products[line.Sku].Id, line.Unit, line.Qty, line.UnitPrice);
            // Submitted a few hours ago, oldest first, so the queue looks like a normal morning.
            po.Submit(branches[demo.DeliverToBranchCode].Id, now.AddHours(-(DemoPurchasing.PendingApproval.Length - i) * 2));
            db.PurchaseOrders.Add(po);
        }
    }
}

/// <summary>Runs <see cref="DemoSeeder"/> at start-up when SEED_DEMO=true (migrations must already be applied).</summary>
internal sealed class DemoSeedHostedService(IServiceScopeFactory scopes, IOptions<DemoOptions> demo) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        if (!demo.Value.Seed) return;
        await using var scope = scopes.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<DemoSeeder>().SeedAsync(ct);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
