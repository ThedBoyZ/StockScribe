using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace StockScribe.Api.Features.Auth;

/// <summary>Bound from the "Auth" section, i.e. env var Auth__JwtSigningKey (JWT_SIGNING_KEY in .env).</summary>
public sealed class AuthOptions
{
    public const string SectionName = "Auth";
    public const string Issuer = "stockscribe";
    public const string Audience = "stockscribe";

    /// <summary>HMAC-SHA256 key; at least 32 bytes. Production value lives only in the VM's .env.</summary>
    [Required, MinLength(32)]
    public string JwtSigningKey { get; init; } = string.Empty;

    public SymmetricSecurityKey SigningKey() => new(Encoding.UTF8.GetBytes(JwtSigningKey));
}

/// <summary>
/// DEMO_MODE=true exposes the persona login (ADR-0003); SEED_DEMO=true seeds demo data at start-up
/// (docs/sa/seed-data.md). Both are off unless set.
/// </summary>
public sealed class DemoOptions
{
    public bool Enabled { get; set; }
    public bool Seed { get; set; }
}
