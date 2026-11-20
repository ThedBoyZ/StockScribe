using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using Microsoft.IdentityModel.JsonWebTokens;
using StockScribe.Api.Domain;
using StockScribe.Api.Features.Auth;

namespace StockScribe.Tests.Unit;

public class SessionTokenServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 20, 1, 0, 0, TimeSpan.Zero);

    private static SessionTokenService Service() => new(
        Options.Create(new AuthOptions { JwtSigningKey = "unit-test-signing-key-at-least-32-bytes!!" }),
        new FakeTimeProvider(Now));

    [Fact]
    public void Token_carries_the_rbac_claims_and_expires_after_8_hours()
    {
        var branch = new Branch("B2", "สาขา 2");
        var user = User.CreatePersona("manager-b2", "นภา ใจดี", UserRole.BranchManager, branch.Id);

        var token = Service().Create(user);
        var jwt = new JsonWebToken(token.Value);

        Assert.Equal(user.Id.ToString(), jwt.Subject);
        Assert.Equal("นภา ใจดี", jwt.GetClaim("name").Value);
        Assert.Equal("BranchManager", jwt.GetClaim("role").Value);
        Assert.Equal(branch.Id.ToString(), jwt.GetClaim("branch_id").Value);
        Assert.Equal(Now.AddHours(8), token.ExpiresAt);
        Assert.Equal(Now.AddHours(8).UtcDateTime, jwt.ValidTo);
        Assert.Equal("HS256", jwt.Alg);
    }

    [Fact]
    public void Roles_without_a_branch_get_no_branch_claim()
    {
        var owner = User.CreatePersona("owner", "คุณสมศรี", UserRole.Owner, branchId: null);

        var jwt = new JsonWebToken(Service().Create(owner).Value);

        Assert.False(jwt.TryGetClaim("branch_id", out _));
    }

    [Fact]
    public void Persona_key_is_required()
    {
        Assert.Throws<ArgumentException>(() => User.CreatePersona(" ", "x", UserRole.Owner, null));
    }
}
