using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StockScribe.Api.Domain;
using StockScribe.Api.Infrastructure.Persistence;
using StockScribe.Api.Seed;

namespace StockScribe.Api.Features.Auth;

public sealed record BranchSummary(Guid Id, string Name);

/// <summary>GET /api/v1/me and the demo-login response.</summary>
public sealed record MeResponse(Guid Id, string Name, string Role, BranchSummary? Branch)
{
    public static MeResponse From(User user) => new(
        user.Id,
        user.DisplayName,
        user.Role.ToString(),
        user.Branch is null ? null : new BranchSummary(user.Branch.Id, user.Branch.Name));
}

public sealed record PersonaResponse(Guid Id, string Key, string Name, string Role, BranchSummary? Branch);

public sealed record DemoLoginRequest(Guid PersonaId);

public static class AuthEndpoints
{
    /// <summary>JWT read from the sf_session cookie, plus the demo seeder.</summary>
    public static IServiceCollection AddStockScribeAuth(this IServiceCollection services)
    {
        services.AddOptions<AuthOptions>()
            .BindConfiguration(AuthOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<DemoOptions>().Configure<IConfiguration>((o, config) =>
        {
            o.Enabled = config.GetValue<bool>("DEMO_MODE");
            o.Seed = config.GetValue<bool>("SEED_DEMO");
        });

        services.AddSingleton<SessionTokenService>();
        services.AddScoped<DemoSeeder>();
        services.AddHostedService<DemoSeedHostedService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        // Configured from options (not builder.Configuration) so test hosts can override the key.
        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<AuthOptions>>((o, auth) =>
            {
                o.MapInboundClaims = false; // keep "sub", "role" as issued
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = AuthOptions.Issuer,
                    ValidAudience = AuthOptions.Audience,
                    IssuerSigningKey = auth.Value.SigningKey(),
                    NameClaimType = SessionClaims.Name,
                    RoleClaimType = SessionClaims.Role,
                    ClockSkew = TimeSpan.FromMinutes(1),
                };
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        ctx.Token = ctx.Request.Cookies[SessionCookie.Name];
                        return Task.CompletedTask;
                    },
                };
            });
        services.AddAuthorization();

        return services;
    }

    /// <summary>/api/v1/auth/* — anonymous. Persona endpoints exist only when DEMO_MODE=true.</summary>
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/api/v1/auth").AllowAnonymous().WithTags("Auth");
        auth.MapGet("/personas", GetPersonas);
        auth.MapPost("/demo-login", DemoLogin);
        auth.MapPost("/logout", Logout);
        return app;
    }

    /// <summary>GET /api/v1/me — on the authenticated /api/v1 group.</summary>
    public static RouteGroupBuilder MapMeEndpoint(this RouteGroupBuilder api)
    {
        api.MapGet("/me", GetMe).WithTags("Auth");
        return api;
    }

    private static async Task<Results<Ok<List<PersonaResponse>>, NotFound>> GetPersonas(
        AppDbContext db, IOptions<DemoOptions> demo, CancellationToken ct)
    {
        if (!demo.Value.Enabled) return TypedResults.NotFound();

        var users = await db.Users.AsNoTracking()
            .Include(u => u.Branch)
            .Where(u => u.PersonaKey != null)
            .ToListAsync(ct);

        return TypedResults.Ok(users
            .OrderBy(u => DemoPersonas.SortOrder(u.PersonaKey))
            .Select(u => new PersonaResponse(
                u.Id,
                u.PersonaKey!,
                u.DisplayName,
                u.Role.ToString(),
                u.Branch is null ? null : new BranchSummary(u.Branch.Id, u.Branch.Name)))
            .ToList());
    }

    private static async Task<Results<Ok<MeResponse>, NotFound, ProblemHttpResult>> DemoLogin(
        DemoLoginRequest request,
        AppDbContext db,
        SessionTokenService tokens,
        IOptions<DemoOptions> demo,
        HttpContext http,
        CancellationToken ct)
    {
        if (!demo.Value.Enabled) return TypedResults.NotFound();

        var user = await db.Users.AsNoTracking()
            .Include(u => u.Branch)
            .SingleOrDefaultAsync(u => u.Id == request.PersonaId && u.PersonaKey != null, ct);
        if (user is null)
            return TypedResults.Problem(title: "ไม่พบผู้ใช้เดโมนี้", statusCode: StatusCodes.Status404NotFound);

        SessionCookie.Append(http.Response, tokens.Create(user));
        return TypedResults.Ok(MeResponse.From(user));
    }

    private static NoContent Logout(HttpContext http)
    {
        SessionCookie.Delete(http.Response);
        return TypedResults.NoContent();
    }

    // Reads the user from the database, so a session for a deleted user (e.g. after a demo reset) ends with 401.
    private static async Task<Results<Ok<MeResponse>, UnauthorizedHttpResult>> GetMe(
        ClaimsPrincipal principal, AppDbContext db, CancellationToken ct)
    {
        if (!Guid.TryParse(principal.FindFirstValue(SessionClaims.Subject), out var userId))
            return TypedResults.Unauthorized();

        var user = await db.Users.AsNoTracking()
            .Include(u => u.Branch)
            .SingleOrDefaultAsync(u => u.Id == userId, ct);

        return user is null ? TypedResults.Unauthorized() : TypedResults.Ok(MeResponse.From(user));
    }
}
