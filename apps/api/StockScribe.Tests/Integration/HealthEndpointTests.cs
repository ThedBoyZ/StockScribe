using System.Net;
using System.Text.Json;

namespace StockScribe.Tests.Integration;

[Collection(IntegrationCollection.Name)]
[Trait("Category", "Integration")]
public class HealthEndpointTests(DatabaseFixture db)
{
    [Fact]
    public async Task Live_returns_200()
    {
        await using var factory = db.CreateFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Ready_returns_200_when_both_databases_are_up()
    {
        await using var factory = db.CreateFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health/ready");
        var body = await ReadJson(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("no-store", response.Headers.CacheControl?.ToString());
        Assert.Equal("Healthy", body.GetProperty("status").GetString());
        Assert.Equal("Healthy", body.GetProperty("checks").GetProperty("postgres").GetProperty("status").GetString());
        Assert.Equal("Healthy", body.GetProperty("checks").GetProperty("mongo").GetProperty("status").GetString());
    }

    [Fact]
    public async Task Ready_returns_503_when_mongo_is_unreachable()
    {
        // Port 1 on loopback: nothing listens, so the ping fails fast.
        await using var factory = db.CreateFactory("mongodb://127.0.0.1:1/?serverSelectionTimeoutMS=500&connectTimeoutMS=500");
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/health/ready");
        var body = await ReadJson(response);

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal("Unhealthy", body.GetProperty("status").GetString());
        Assert.Equal("Healthy", body.GetProperty("checks").GetProperty("postgres").GetProperty("status").GetString());
        Assert.Equal("Unhealthy", body.GetProperty("checks").GetProperty("mongo").GetProperty("status").GetString());
        // Public response must not leak exception details or hosts.
        Assert.DoesNotContain("127.0.0.1", body.GetRawText());
    }

    [Fact]
    public async Task Unknown_route_returns_problem_details_with_trace_id()
    {
        await using var factory = db.CreateFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/does-not-exist");
        var body = await ReadJson(response);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(404, body.GetProperty("status").GetInt32());
        Assert.False(string.IsNullOrEmpty(body.GetProperty("traceId").GetString()));
    }

    private static async Task<JsonElement> ReadJson(HttpResponseMessage response) =>
        JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
}
