namespace StockScribe.Api.Domain;

/// <summary>docs/sa/state-machines.md. Stored as text, so do not rename members.</summary>
public enum PurchaseOrderStatus
{
    Draft,
    PendingApproval,
    Approved,
    Sent,
    PartiallyReceived,
    Closed,
    Cancelled,
}

/// <summary>Who is acting, from the session claims.</summary>
public sealed record Actor(Guid UserId, string Name, UserRole Role, Guid? BranchId);

/// <summary>What the API tells the screen about approving this PO for this user (api-spec `approval`).</summary>
public sealed record ApprovalCheck(
    bool CanApprove,
    UserRole RequiredRole,
    decimal Limit,
    decimal? Remaining,
    string? Message,
    string? DeniedType);

/// <summary>
/// Purchase order. Approval rules (US-002, Q-SA-01): a branch manager may approve a PO that goes
/// only to their own branch with a subtotal (before VAT) of at most ฿5,000; everything else waits for the owner.
/// </summary>
public sealed class PurchaseOrder
{
    public const int NumberMaxLength = 20;
    public const int RejectReasonMaxLength = 500;
    public const decimal DefaultVatRate = 0.07m;
    public const decimal ManagerApprovalLimit = 5000.00m;

    private readonly List<PurchaseOrderLine> _lines = [];

    public Guid Id { get; private set; }
    public string Number { get; private set; } = null!;
    public Guid SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = null!;
    public PurchaseOrderStatus Status { get; private set; }
    public decimal Subtotal { get; private set; }
    public decimal VatRate { get; private set; }
    public decimal VatAmount { get; private set; }
    public decimal Total { get; private set; }

    /// <summary>Set on submit: BranchManager or Owner.</summary>
    public UserRole? RequiredApproverRole { get; private set; }
    /// <summary>The branch whose manager may approve (only when the PO goes to one branch).</summary>
    public Guid? ApprovalBranchId { get; private set; }
    public Branch? ApprovalBranch { get; private set; }

    public DateTimeOffset? SubmittedAt { get; private set; }
    public DateTimeOffset? ApprovedAt { get; private set; }
    public Guid? ApprovedById { get; private set; }
    public User? ApprovedBy { get; private set; }
    public string? RejectReason { get; private set; }

    /// <summary>PostgreSQL xmin; clients send it back so a stale screen gets 409 instead of overwriting.</summary>
    public uint Version { get; private set; }

    public IReadOnlyList<PurchaseOrderLine> Lines => _lines;

    private PurchaseOrder() { } // EF Core

    public PurchaseOrder(string number, Guid supplierId, decimal vatRate = DefaultVatRate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(number.Length, NumberMaxLength, nameof(number));

        Id = Guid.CreateVersion7();
        Number = number;
        SupplierId = supplierId;
        VatRate = vatRate;
        Status = PurchaseOrderStatus.Draft;
    }

    public PurchaseOrderLine AddLine(Guid productId, string unitName, int qty, decimal unitPrice)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw DomainException.InvalidTransition("แก้รายการได้เฉพาะ PO ที่เป็นร่าง");

        var line = new PurchaseOrderLine(Id, productId, unitName, qty, unitPrice);
        _lines.Add(line);
        Recalculate();
        return line;
    }

    /// <summary>Draft → PendingApproval. Decides who must approve.</summary>
    public void Submit(Guid? singleDeliveryBranchId, DateTimeOffset now)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw DomainException.InvalidTransition("ส่งขออนุมัติได้เฉพาะ PO ที่เป็นร่าง");
        if (_lines.Count == 0)
            throw DomainException.Invalid("empty-purchase-order", "PO ต้องมีอย่างน้อย 1 รายการ");

        ApprovalBranchId = singleDeliveryBranchId;
        RequiredApproverRole = singleDeliveryBranchId is not null && Subtotal <= ManagerApprovalLimit
            ? UserRole.BranchManager
            : UserRole.Owner;
        SubmittedAt = now;
        Status = PurchaseOrderStatus.PendingApproval;
    }

    public ApprovalCheck CheckApproval(Actor actor)
    {
        var required = RequiredApproverRole ?? UserRole.Owner;
        var remaining = ManagerApprovalLimit - Subtotal;

        if (Status != PurchaseOrderStatus.PendingApproval)
            return new(false, required, ManagerApprovalLimit, remaining, "PO นี้ไม่ได้รออนุมัติ", "invalid-state-transition");

        return actor.Role switch
        {
            UserRole.Owner => new(true, required, ManagerApprovalLimit, remaining, null, null),
            UserRole.BranchManager when ApprovalBranchId is null || ApprovalBranchId != actor.BranchId =>
                new(false, required, ManagerApprovalLimit, remaining,
                    "PO นี้ไม่ได้ส่งถึงสาขาของคุณ · รอเจ้าของอนุมัติ", "approval-branch-mismatch"),
            UserRole.BranchManager when required != UserRole.BranchManager || Subtotal > ManagerApprovalLimit =>
                new(false, required, ManagerApprovalLimit, remaining,
                    "เกินวงเงิน ฿5,000 · รอเจ้าของอนุมัติ", "approval-limit-exceeded"),
            UserRole.BranchManager => new(true, required, ManagerApprovalLimit, remaining, null, null),
            _ => new(false, required, ManagerApprovalLimit, remaining, "ไม่มีสิทธิ์อนุมัติ PO", "forbidden"),
        };
    }

    /// <summary>PendingApproval → Approved (US-002 AC 1, 2, 4).</summary>
    public void Approve(Actor actor, DateTimeOffset now)
    {
        EnsureDecisionAllowed(actor);
        Status = PurchaseOrderStatus.Approved;
        ApprovedAt = now;
        ApprovedById = actor.UserId;
        RejectReason = null;
    }

    /// <summary>PendingApproval → Draft with a reason, so purchasing can fix and resubmit (W02-A).</summary>
    public void Reject(Actor actor, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw DomainException.Invalid("reason-required", "กรุณาระบุเหตุผลที่ตีกลับ");
        if (reason.Length > RejectReasonMaxLength)
            throw DomainException.Invalid("reason-too-long", $"เหตุผลยาวได้ไม่เกิน {RejectReasonMaxLength} ตัวอักษร");

        EnsureDecisionAllowed(actor);
        Status = PurchaseOrderStatus.Draft;
        RejectReason = reason.Trim();
        RequiredApproverRole = null;
        ApprovalBranchId = null;
        SubmittedAt = null;
    }

    private void EnsureDecisionAllowed(Actor actor)
    {
        if (Status != PurchaseOrderStatus.PendingApproval)
            throw DomainException.InvalidTransition($"PO {Number} ไม่ได้อยู่ในสถานะรออนุมัติ");

        var check = CheckApproval(actor);
        if (!check.CanApprove)
            throw DomainException.Forbidden(check.DeniedType!, check.Message!);
    }

    private void Recalculate()
    {
        Subtotal = _lines.Sum(l => l.LineTotal);
        VatAmount = Money.Round(Subtotal * VatRate);
        Total = Subtotal + VatAmount;
    }
}

public sealed class PurchaseOrderLine
{
    public const int UnitNameMaxLength = 30;

    public Guid Id { get; private set; }
    public Guid PurchaseOrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public string UnitName { get; private set; } = null!;
    public int QtyOrdered { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal LineTotal { get; private set; }

    private PurchaseOrderLine() { } // EF Core

    internal PurchaseOrderLine(Guid purchaseOrderId, Guid productId, string unitName, int qty, decimal unitPrice)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(unitName);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(unitName.Length, UnitNameMaxLength, nameof(unitName));
        if (qty <= 0) throw DomainException.Invalid("invalid-quantity", "จำนวนต้องมากกว่า 0");
        if (unitPrice < 0) throw DomainException.Invalid("invalid-price", "ราคาต้องไม่ติดลบ");

        Id = Guid.CreateVersion7();
        PurchaseOrderId = purchaseOrderId;
        ProductId = productId;
        UnitName = unitName;
        QtyOrdered = qty;
        UnitPrice = unitPrice;
        LineTotal = Money.Round(qty * unitPrice);
    }
}

/// <summary>ADR-0005: numeric(12,2), round half away from zero.</summary>
public static class Money
{
    public static decimal Round(decimal amount) => Math.Round(amount, 2, MidpointRounding.AwayFromZero);
}
