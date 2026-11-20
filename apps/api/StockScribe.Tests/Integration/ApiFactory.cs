using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using StockScribe.Api.Infrastructure.Persistence;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;

namespace StockScribe.Tests.Integration;

/// <summary>
/// Real PostgreSQL + MongoDB in throwaway containers (needs Docker), shared by every
/// test in <see cref="IntegrationCollection"/>. Containers start once and are removed afterwards.
/// </summary>
public sealed class DatabaseFixture : IAsyncLifetime
{
    // Same major versions as docker-compose.yml.
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17-alpine").Build();
    private readonly MongoDbContainer _mongo = new MongoDbBuilder("mongo:8").Build();

    public string PostgresConnectionString => _postgres.GetConnectionString();
    public string MongoConnectionString => _mongo.GetConnectionString();

    public async Task InitializeAsync()
    {
        await Task.WhenAll(_postgres.StartAsync(), _mongo.StartAsync());

        // Schema first, so hosts that seed demo data at start-up (SEED_DEMO=true) find their tables.
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(PostgresConnectionString)
            .UseSnakeCaseNamingConvention()
            .Options;
        await using var context = new AppDbContext(options);
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
        await _mongo.DisposeAsync();
    }

    /// <summary>
    /// A test host wired to the containers. Override Mongo to simulate an outage;
    /// <paramref name="settings"/> adds or overrides configuration (e.g. DEMO_MODE).
    /// </summary>
    public WebApplicationFactory<Program> CreateFactory(
        string? mongoConnectionString = null,
        IReadOnlyDictionary<string, string?>? settings = null) =>
        new ApiFactory(PostgresConnectionString, mongoConnectionString ?? MongoConnectionString, settings);
}

/// <summary>
/// Runs these tests one class at a time: they share containers, and the API's Serilog
/// bootstrap logger is process-wide.
/// </summary>
[CollectionDefinition(Name)]
public sealed class IntegrationCollection : ICollectionFixture<DatabaseFixture>
{
    public const string Name = "Integration";
}

internal sealed class ApiFactory(string postgres, string mongo, IReadOnlyDictionary<string, string?>? settings)
    : WebApplicationFactory<Program>
{
    public const string SigningKey = "test-signing-key-at-least-32-bytes-long-0123456789";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // "Testing" skips appsettings.Development.json (localhost:5433), so nothing leaks from a dev setup.
        builder.UseEnvironment("Testing");
        var values = new Dictionary<string, string?>
        {
            ["ConnectionStrings:Postgres"] = postgres,
            ["Mongo:ConnectionString"] = mongo,
            ["Mongo:Database"] = "stockscribe_tests",
            ["Auth:JwtSigningKey"] = SigningKey,
            ["Serilog:MinimumLevel:Default"] = "Warning",
        };
        foreach (var (key, value) in settings ?? new Dictionary<string, string?>())
            values[key] = value;
        builder.ConfigureAppConfiguration(config => config.AddInMemoryCollection(values));
    }
}
