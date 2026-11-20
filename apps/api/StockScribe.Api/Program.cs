using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using Serilog.Formatting.Compact;
using StockScribe.Api.Features.Auth;
using StockScribe.Api.Infrastructure.Mongo;
using StockScribe.Api.Infrastructure.Persistence;

// Bootstrap logger so startup failures are still logged as JSON.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ---- Logging: Serilog JSON to stdout ----
    builder.Services.AddSerilog((services, config) => config
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console(new RenderedCompactJsonFormatter()));

    // ---- Configuration (env: ConnectionStrings__Postgres, Mongo__ConnectionString, Mongo__Database) ----
    // Connection string is resolved lazily so test hosts (WebApplicationFactory) can override it.
    builder.Services.AddDbContext<AppDbContext>((sp, options) => options
        .UseNpgsql(sp.GetRequiredService<IConfiguration>().GetConnectionString("Postgres")
            ?? throw new InvalidOperationException(
                "Missing connection string 'Postgres' (env ConnectionStrings__Postgres)."))
        .UseSnakeCaseNamingConvention());

    builder.Services.AddOptions<MongoOptions>()
        .BindConfiguration(MongoOptions.SectionName)
        .ValidateDataAnnotations()
        .ValidateOnStart();
    builder.Services.AddSingleton<IMongoClient>(sp =>
        new MongoClient(sp.GetRequiredService<IOptions<MongoOptions>>().Value.ConnectionString));
    builder.Services.AddSingleton(sp =>
        sp.GetRequiredService<IMongoClient>()
          .GetDatabase(sp.GetRequiredService<IOptions<MongoOptions>>().Value.Database));

    // ---- Health checks (no third-party HealthChecks packages) ----
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<AppDbContext>("postgres", tags: [ReadyTag])
        .AddCheck<MongoHealthCheck>("mongo", tags: [ReadyTag]);
    // 5 s per check: enough for Neon to wake from suspend, short enough for deploy probes.
    builder.Services.Configure<HealthCheckServiceOptions>(o =>
    {
        foreach (var registration in o.Registrations)
            registration.Timeout = TimeSpan.FromSeconds(5);
    });

    // ---- Time ----
    builder.Services.AddSingleton(TimeProvider.System);

    // ---- Auth: demo persona login → JWT in the sf_session cookie (ADR-0003) + demo seed ----
    builder.Services.AddStockScribeAuth();

    // ---- Errors: RFC 9457 ProblemDetails with traceId ----
    builder.Services.AddProblemDetails(options =>
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Instance ??= ctx.HttpContext.Request.Path;
            ctx.ProblemDetails.Extensions["traceId"] =
                Activity.Current?.TraceId.ToString() ?? ctx.HttpContext.TraceIdentifier;
        });

    // ---- OpenAPI (served only in Development) ----
    builder.Services.AddOpenApi();

    var app = builder.Build();

    app.UseExceptionHandler();   // unhandled exceptions -> 500 ProblemDetails
    app.UseStatusCodePages();    // empty 4xx/5xx (e.g. 404) -> ProblemDetails
    app.UseSerilogRequestLogging();
    app.UseAuthentication();
    app.UseAuthorization();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();        // GET /openapi/v1.json
    }

    // Liveness: process is up. Runs no checks, so it never touches the databases.
    app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });

    // Readiness: Postgres + Mongo. 200 when all Healthy, 503 otherwise (Degraded counts as not ready).
    app.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains(ReadyTag),
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
        },
        ResponseWriter = WriteHealthJson,
    });

    // ---- API ----
    // /api/v1/auth/* is anonymous; everything else under /api/v1 needs a session (401 ProblemDetails otherwise).
    // A group policy (not a fallback policy) keeps unknown routes at 404.
    app.MapAuthEndpoints();
    var api = app.MapGroup("/api/v1").RequireAuthorization();
    api.MapMeEndpoint();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "API terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

// Per-check status only. Failure details (exception messages, hosts) go to the log, not the public response.
static Task WriteHealthJson(HttpContext context, HealthReport report)
{
    context.Response.Headers.CacheControl = "no-store";
    return context.Response.WriteAsJsonAsync(new
    {
        status = report.Status.ToString(),
        durationMs = Math.Round(report.TotalDuration.TotalMilliseconds),
        checks = report.Entries.ToDictionary(
            e => e.Key,
            e => new
            {
                status = e.Value.Status.ToString(),
                durationMs = Math.Round(e.Value.Duration.TotalMilliseconds),
            }),
    });
}

public partial class Program
{
    private const string ReadyTag = "ready";
}
