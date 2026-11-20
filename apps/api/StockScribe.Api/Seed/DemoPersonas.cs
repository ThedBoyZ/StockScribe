using StockScribe.Api.Domain;

namespace StockScribe.Api.Seed;

public sealed record DemoPersona(string Key, string Name, UserRole Role, string? BranchCode);

/// <summary>Demo branches and personas from docs/sa/seed-data.md. Names are fictional.</summary>
public static class DemoPersonas
{
    public static readonly IReadOnlyList<(string Code, string Name)> Branches =
    [
        ("B1", "สาขา 1"),
        ("B2", "สาขา 2"),
        ("B3", "สาขา 3"),
    ];

    /// <summary>In the order the login page shows them.</summary>
    public static readonly IReadOnlyList<DemoPersona> All =
    [
        new("manager-b2", "นภา ใจดี", UserRole.BranchManager, "B2"),
        new("purchasing", "วิทย์ สมบูรณ์", UserRole.Purchasing, null),
        new("staff-b2", "สมชาย รักงาน", UserRole.Staff, "B2"),
        new("owner", "คุณสมศรี", UserRole.Owner, null),
    ];

    /// <summary>Position on the login page; unknown keys go last.</summary>
    public static int SortOrder(string? key)
    {
        for (var i = 0; i < All.Count; i++)
            if (All[i].Key == key) return i;
        return int.MaxValue;
    }
}
