namespace StockScribe.Api.Domain;

/// <summary>A store branch, e.g. code "B2", name "สาขา 2".</summary>
public sealed class Branch
{
    public const int CodeMaxLength = 10;
    public const int NameMaxLength = 100;

    public Guid Id { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    private Branch() { } // EF Core

    public Branch(string code, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(code.Length, CodeMaxLength, nameof(code));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(name.Length, NameMaxLength, nameof(name));

        Id = Guid.CreateVersion7();
        Code = code;
        Name = name;
    }
}
