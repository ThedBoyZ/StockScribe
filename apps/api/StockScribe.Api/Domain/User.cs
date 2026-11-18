namespace StockScribe.Api.Domain;

/// <summary>
/// An application user / demo persona. Rule (also enforced by a DB CHECK constraint):
/// Staff and BranchManager must have a branch.
/// </summary>
public sealed class User
{
    public const int DisplayNameMaxLength = 100;

    public Guid Id { get; private set; }
    public string DisplayName { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public Guid? BranchId { get; private set; }
    public Branch? Branch { get; private set; }
    public bool IsDemo { get; private set; }

    private User() { } // EF Core

    public User(string displayName, UserRole role, Guid? branchId, bool isDemo = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(displayName.Length, DisplayNameMaxLength, nameof(displayName));
        if (!Enum.IsDefined(role))
            throw new ArgumentOutOfRangeException(nameof(role), role, "Unknown role.");
        if (role.RequiresBranch() && branchId is null)
            throw new ArgumentException($"Role {role} requires a branch.", nameof(branchId));

        Id = Guid.CreateVersion7();
        DisplayName = displayName;
        Role = role;
        BranchId = branchId;
        IsDemo = isDemo;
    }
}
