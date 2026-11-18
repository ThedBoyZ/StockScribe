using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
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

    public Task InitializeAsync() => Task.WhenAll(_postgres.StartAsync(), _mongo.StartAsync());

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
        await _mongo.DisposeAsync();
    }

    /// <summary>A test host wired to the containers. Override Mongo to simulate an outage.</summary>
    public WebApplicationFactory<Program> CreateFactory(string? mongoConnectionString = null) =>
        new ApiFactory(PostgresConnectionString, mongoConnectionString ?? MongoConnectionString);
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

internal sealed class ApiFactory(string postgres, string mongo) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // "Testing" skips appsettings.Development.json (localhost:5433), so nothing leaks from a dev setup.
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration(config => config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:Postgres"] = postgres,
            ["Mongo:ConnectionString"] = mongo,
            ["Mongo:Database"] = "stockscribe_tests",
            ["Serilog:MinimumLevel:Default"] = "Warning",
        }));
    }
}
