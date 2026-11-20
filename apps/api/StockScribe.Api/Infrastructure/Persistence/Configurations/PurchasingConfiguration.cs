using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockScribe.Api.Domain;

namespace StockScribe.Api.Infrastructure.Persistence.Configurations;

internal sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> b)
    {
        b.ToTable("suppliers");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.Name).HasMaxLength(Supplier.NameMaxLength).IsRequired();
        b.HasIndex(x => x.Name).IsUnique();
        b.Property(x => x.MinOrderAmount).HasPrecision(12, 2);
    }
}

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {
        b.ToTable("products");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.Sku).HasMaxLength(Product.SkuMaxLength).IsRequired();
        b.HasIndex(x => x.Sku).IsUnique();
        b.Property(x => x.Name).HasMaxLength(Product.NameMaxLength).IsRequired();
    }
}

internal sealed class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> b)
    {
        b.ToTable("purchase_orders");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();

        b.Property(x => x.Number).HasMaxLength(PurchaseOrder.NumberMaxLength).IsRequired();
        b.HasIndex(x => x.Number).IsUnique();
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Status);

        b.Property(x => x.Subtotal).HasPrecision(12, 2);
        b.Property(x => x.VatRate).HasPrecision(5, 4);
        b.Property(x => x.VatAmount).HasPrecision(12, 2);
        b.Property(x => x.Total).HasPrecision(12, 2);

        b.Property(x => x.RequiredApproverRole).HasConversion<string>().HasMaxLength(20);
        b.Property(x => x.RejectReason).HasMaxLength(PurchaseOrder.RejectReasonMaxLength);

        // Npgsql maps a uint row version to PostgreSQL's system column xmin (no extra column).
        b.Property(x => x.Version).IsRowVersion();

        b.HasOne(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.ApprovalBranch).WithMany().HasForeignKey(x => x.ApprovalBranchId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.ApprovedBy).WithMany().HasForeignKey(x => x.ApprovedById).OnDelete(DeleteBehavior.Restrict);

        b.HasMany(x => x.Lines).WithOne().HasForeignKey(l => l.PurchaseOrderId).OnDelete(DeleteBehavior.Cascade);
        b.Navigation(x => x.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

internal sealed class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderLine> b)
    {
        b.ToTable("po_lines", t => t.HasCheckConstraint("ck_po_lines_qty_positive", "qty_ordered > 0"));
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.PurchaseOrderId).HasColumnName("po_id");
        b.Property(x => x.UnitName).HasMaxLength(PurchaseOrderLine.UnitNameMaxLength).IsRequired();
        b.Property(x => x.UnitPrice).HasPrecision(12, 2);
        b.Property(x => x.LineTotal).HasPrecision(12, 2);
        b.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
    }
}
