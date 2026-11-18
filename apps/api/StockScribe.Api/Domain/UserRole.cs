namespace StockScribe.Api.Domain;

/// <summary>Roles from docs/sa/rbac.md. Stored as text (varchar(20)), so do not rename members.</summary>
public enum UserRole
{
    Staff,
    BranchManager,
    Purchasing,
    Owner,
    Admin,
}

public static class UserRoleRules
{
    /// <summary>Staff and BranchManager belong to one branch; other roles see every branch.</summary>
    public static bool RequiresBranch(this UserRole role) =>
        role is UserRole.Staff or UserRole.BranchManager;
}
