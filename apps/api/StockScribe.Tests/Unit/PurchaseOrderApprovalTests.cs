using StockScribe.Api.Domain;

namespace StockScribe.Tests.Unit;

/// <summary>US-002 approval rules (BR: manager limit ฿5,000 before VAT, own branch only).</summary>
public class PurchaseOrderApprovalTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 20, 2, 0, 0, TimeSpan.Zero);
    private static readonly Guid BranchB2 = Guid.CreateVersion7();
    private static readonly Actor Manager = new(Guid.CreateVersion7(), "นภา ใจดี", UserRole.BranchManager, BranchB2);
    private static readonly Actor OtherManager = new(Guid.CreateVersion7(), "ผจก. สาขา 3", UserRole.BranchManager, Guid.CreateVersion7());
    private static readonly Actor Owner = new(Guid.CreateVersion7(), "คุณสมศรี", UserRole.Owner, null);
    private static readonly Actor Staff = new(Guid.CreateVersion7(), "สมชาย", UserRole.Staff, BranchB2);

    private static PurchaseOrder Pending(decimal subtotal, Guid? branch = null)
    {
        var po = new PurchaseOrder("PO-TEST", Guid.CreateVersion7());
        po.AddLine(Guid.CreateVersion7(), "ลัง", 1, subtotal);
        po.Submit(branch ?? BranchB2, Now.AddHours(-1));
        return po;
    }

    [Theory]
    [InlineData("4200.00")]
    [InlineData("5000.00")] // AC 2: exactly the limit is allowed
    public void Manager_approves_own_branch_po_within_the_limit(string subtotal)
    {
        var po = Pending(decimal.Parse(subtotal));

        po.Approve(Manager, Now);

        Assert.Equal(PurchaseOrderStatus.Approved, po.Status);
        Assert.Equal(Manager.UserId, po.ApprovedById);
        Assert.Equal(Now, po.ApprovedAt);
    }

    [Fact]
    public void Manager_cannot_approve_above_the_limit()
    {
        var po = Pending(5000.01m);

        var ex = Assert.Throws<DomainException>(() => po.Approve(Manager, Now));

        Assert.Equal("approval-limit-exceeded", ex.Type);
        Assert.Equal(403, ex.StatusCode);
        Assert.Equal(UserRole.Owner, po.RequiredApproverRole);
        Assert.Equal(PurchaseOrderStatus.PendingApproval, po.Status);
    }

    [Fact]
    public void Manager_cannot_approve_another_branch()
    {
        var ex = Assert.Throws<DomainException>(() => Pending(1000m).Approve(OtherManager, Now));
        Assert.Equal("approval-branch-mismatch", ex.Type);
    }

    [Fact]
    public void Multi_branch_po_always_needs_the_owner()
    {
        var po = new PurchaseOrder("PO-MULTI", Guid.CreateVersion7());
        po.AddLine(Guid.CreateVersion7(), "ลัง", 1, 100m);
        po.Submit(singleDeliveryBranchId: null, Now);

        Assert.Equal(UserRole.Owner, po.RequiredApproverRole);
        Assert.False(po.CheckApproval(Manager).CanApprove);
        Assert.True(po.CheckApproval(Owner).CanApprove);
    }

    [Fact]
    public void Owner_approves_any_amount_and_staff_cannot()
    {
        Assert.Throws<DomainException>(() => Pending(100m).Approve(Staff, Now));

        var po = Pending(8500m);
        po.Approve(Owner, Now);
        Assert.Equal(PurchaseOrderStatus.Approved, po.Status);
    }

    [Fact]
    public void Approving_twice_is_an_invalid_transition()
    {
        var po = Pending(1000m);
        po.Approve(Manager, Now);

        var ex = Assert.Throws<DomainException>(() => po.Approve(Manager, Now));
        Assert.Equal(409, ex.StatusCode);
    }

    [Fact]
    public void Reject_needs_a_reason_and_returns_the_po_to_draft()
    {
        var po = Pending(1000m);

        Assert.Equal("reason-required", Assert.Throws<DomainException>(() => po.Reject(Manager, "  ")).Type);

        po.Reject(Manager, "ราคาน้ำดื่มสูงกว่ารอบก่อน");
        Assert.Equal(PurchaseOrderStatus.Draft, po.Status);
        Assert.Equal("ราคาน้ำดื่มสูงกว่ารอบก่อน", po.RejectReason);
        Assert.Null(po.RequiredApproverRole);
    }

    [Fact]
    public void Totals_use_7_percent_vat_rounded_half_away_from_zero()
    {
        var po = new PurchaseOrder("PO-VAT", Guid.CreateVersion7());
        po.AddLine(Guid.CreateVersion7(), "แพ็ค 12", 12, 60m);
        po.AddLine(Guid.CreateVersion7(), "แพ็ค 12", 12, 250m);
        po.AddLine(Guid.CreateVersion7(), "แพ็ค 6", 2, 240m);

        Assert.Equal(4200.00m, po.Subtotal);
        Assert.Equal(294.00m, po.VatAmount);
        Assert.Equal(4494.00m, po.Total);
        Assert.Equal(0.04m, Money.Round(0.035m));
    }
}
