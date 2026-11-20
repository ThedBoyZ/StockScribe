namespace StockScribe.Api.Domain;

/// <summary>
/// An application user / demo persona. Rule (also enforced by a DB CHECK constraint):
/// Staff and BranchManager must have a branch.
/// </summary>
public sealed class User
{
    public const int DisplayNameMaxLength = 100;
    public const int PersonaKeyMaxLength = 40;

    public Guid Id { get; private set; }
    public string DisplayName { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public Guid? BranchId { get; private set; }
    public Branch? Branch { get; private set; }
    public bool IsDemo { get; private set; }

    /// <summary>
    /// Stable id of a demo-login persona (e.g. "manager-b2"); null for every other user.
    /// The seeder matches on it, so renaming a persona never creates a duplicate.
    /// </summary>
    public string? PersonaKey { get; private set; }

    private User() { } // EF Core

    /// <summary>A user shown on the demo login page (ADR-0003).</summary>
    public static User CreatePersona(string personaKey, string displayName, UserRole role, Guid? branchId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(personaKey);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(personaKey.Length, PersonaKeyMaxLength, nameof(personaKey));

        return new User(displayName, role, branchId, isDemo: true) { PersonaKey = personaKey };
    }

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
