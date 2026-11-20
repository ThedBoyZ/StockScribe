using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using StockScribe.Api.Domain;

namespace StockScribe.Api.Features.Auth;

/// <summary>Claim names in the session JWT (docs/sa/rbac.md).</summary>
public static class SessionClaims
{
    public const string Subject = "sub";
    public const string Name = "name";
    public const string Role = "role";
    public const string BranchId = "branch_id";
}

public sealed record SessionToken(string Value, DateTimeOffset ExpiresAt);

/// <summary>Issues the 8-hour session JWT. Swapping demo login for real login only changes who calls this.</summary>
public sealed class SessionTokenService(IOptions<AuthOptions> options, TimeProvider time)
{
    public static readonly TimeSpan Lifetime = TimeSpan.FromHours(8);

    private static readonly JsonWebTokenHandler Handler = new();

    public SessionToken Create(User user)
    {
        var now = time.GetUtcNow();
        var expires = now + Lifetime;

        var claims = new Dictionary<string, object>
        {
            [SessionClaims.Subject] = user.Id.ToString(),
            [SessionClaims.Name] = user.DisplayName,
            [SessionClaims.Role] = user.Role.ToString(),
        };
        if (user.BranchId is { } branchId)
            claims[SessionClaims.BranchId] = branchId.ToString();

        var token = Handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = AuthOptions.Issuer,
            Audience = AuthOptions.Audience,
            IssuedAt = now.UtcDateTime,
            NotBefore = now.UtcDateTime,
            Expires = expires.UtcDateTime,
            Claims = claims,
            SigningCredentials = new SigningCredentials(options.Value.SigningKey(), SecurityAlgorithms.HmacSha256),
        });

        return new SessionToken(token, expires);
    }
}

/// <summary>
/// The session lives in an HttpOnly cookie so JavaScript cannot read the token (ADR-0003).
/// SameSite=Lax + same-origin API + state changes only via POST/PUT/PATCH/DELETE covers CSRF.
/// </summary>
public static class SessionCookie
{
    public const string Name = "sf_session";

    public static void Append(HttpResponse response, SessionToken token) =>
        response.Cookies.Append(Name, token.Value, Options(token.ExpiresAt));

    public static void Delete(HttpResponse response) =>
        response.Cookies.Delete(Name, Options(expires: null));

    private static CookieOptions Options(DateTimeOffset? expires) => new()
    {
        HttpOnly = true,
        Secure = true, // browsers also accept Secure cookies on http://localhost
        SameSite = SameSiteMode.Lax,
        Path = "/",
        Expires = expires,
        IsEssential = true,
    };
}
