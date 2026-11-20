namespace StockScribe.Api.Domain;

/// <summary>A supplier; one PO goes to exactly one supplier (BR-05).</summary>
public sealed class Supplier
{
    public const int NameMaxLength = 200;

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    /// <summary>Minimum order before VAT (US-005 AC 7).</summary>
    public decimal MinOrderAmount { get; private set; }

    private Supplier() { } // EF Core

    public Supplier(string name, decimal minOrderAmount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(name.Length, NameMaxLength, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegative(minOrderAmount);

        Id = Guid.CreateVersion7();
        Name = name;
        MinOrderAmount = minOrderAmount;
    }
}

/// <summary>
/// A product. Units and conversion factors (docs/sa/erd.md `units`) arrive with US-004 in Slice 5;
/// until then a PO line stores its purchase unit name.
/// </summary>
public sealed class Product
{
    public const int SkuMaxLength = 30;
    public const int NameMaxLength = 200;

    public Guid Id { get; private set; }
    public string Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    private Product() { } // EF Core

    public Product(string sku, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sku);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(sku.Length, SkuMaxLength, nameof(sku));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(name.Length, NameMaxLength, nameof(name));

        Id = Guid.CreateVersion7();
        Sku = sku;
        Name = name;
    }
}
