using StockScribe.Api.Domain;

namespace StockScribe.Tests.Unit;

public class BranchTests
{
    [Fact]
    public void Creates_branch_with_v7_id()
    {
        var branch = new Branch("B2", "สาขา 2");

        Assert.Equal("B2", branch.Code);
        Assert.Equal("สาขา 2", branch.Name);
        Assert.Equal(7, branch.Id.Version);
    }

    [Theory]
    [InlineData("", "สาขา 2")]
    [InlineData("  ", "สาขา 2")]
    [InlineData("B2", "")]
    public void Rejects_blank_code_or_name(string code, string name) =>
        Assert.ThrowsAny<ArgumentException>(() => new Branch(code, name));

    [Fact]
    public void Rejects_code_longer_than_the_column() =>
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Branch(new string('B', Branch.CodeMaxLength + 1), "สาขา"));
}
