using StockScribe.Api.Domain;

namespace StockScribe.Tests.Unit;

public class UserRoleRuleTests
{
    [Theory]
    [InlineData(UserRole.Staff, true)]
    [InlineData(UserRole.BranchManager, true)]
    [InlineData(UserRole.Purchasing, false)]
    [InlineData(UserRole.Owner, false)]
    [InlineData(UserRole.Admin, false)]
    public void RequiresBranch_matches_rbac(UserRole role, bool expected) =>
        Assert.Equal(expected, role.RequiresBranch());

    [Fact]
    public void Every_role_is_covered_by_the_rule_tests()
    {
        // If a role is added, decide its branch rule and add it to the theory above.
        Assert.Equal(5, Enum.GetValues<UserRole>().Length);
    }

    [Theory]
    [InlineData(UserRole.Staff)]
    [InlineData(UserRole.BranchManager)]
    public void Branch_roles_cannot_be_created_without_a_branch(UserRole role) =>
        Assert.Throws<ArgumentException>(() => new User("Somchai", role, branchId: null));

    [Theory]
    [InlineData(UserRole.Purchasing)]
    [InlineData(UserRole.Owner)]
    [InlineData(UserRole.Admin)]
    public void Cross_branch_roles_can_be_created_without_a_branch(UserRole role)
    {
        var user = new User("Somchai", role, branchId: null);

        Assert.Null(user.BranchId);
        Assert.Equal(7, user.Id.Version);           // Guid.CreateVersion7
    }

    [Fact]
    public void Unknown_role_is_rejected() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new User("Somchai", (UserRole)99, Guid.NewGuid()));
}
