using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace StockScribe.Tests.Integration;

/// <summary>Slice 1: demo persona login → sf_session cookie → /api/v1/me (ADR-0003).</summary>
[Collection(IntegrationCollection.Name)]
[Trait("Category", "Integration")]
public class AuthEndpointTests(DatabaseFixture db) : IAsyncLifetime
{
    private static readonly Dictionary<string, string?> DemoOn = new()
    {
        ["DEMO_MODE"] = "true",
        ["SEED_DEMO"] = "true",
    };

    private WebApplicationFactory<Program> _factory = null!;

    public Task InitializeAsync()
    {
        _factory = db.CreateFactory(settings: DemoOn);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync() => await _factory.DisposeAsync();

    // https: the session cookie is Secure, so the client only sends it back over https.
    private HttpClient NewClient(bool handleCookies = true) => _factory.CreateClient(new WebApplicationFactoryClientOptions
    {
        BaseAddress = new Uri("https://localhost"),
        HandleCookies = handleCookies,
    });

    [Fact]
    public async Task Personas_lists_the_four_demo_personas_in_login_page_order()
    {
        using var client = NewClient();

        var personas = await GetPersonas(client);

        Assert.Equal(
            ["นภา ใจดี", "วิทย์ สมบูรณ์", "สมชาย รักงาน", "คุณสมศรี"],
            personas.Select(p => p.GetProperty("name").GetString()));
        Assert.Equal("BranchManager", personas[0].GetProperty("role").GetString());
        Assert.Equal("สาขา 2", personas[0].GetProperty("branch").GetProperty("name").GetString());
        Assert.Equal(JsonValueKind.Null, personas[1].GetProperty("branch").ValueKind);
    }

    [Fact]
    public async Task Demo_login_sets_an_http_only_secure_lax_session_cookie()
    {
        using var client = NewClient();
        var manager = await PersonaId(client, "manager-b2");

        var response = await client.PostAsJsonAsync("/api/v1/auth/demo-login", new { personaId = manager });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var cookie = Assert.Single(response.Headers.GetValues("Set-Cookie"), c => c.StartsWith("sf_session="));
        Assert.Contains("httponly", cookie, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("secure", cookie, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("samesite=lax", cookie, StringComparison.OrdinalIgnoreCase);

        var me = await ReadJson(response);
        Assert.Equal("นภา ใจดี", me.GetProperty("name").GetString());
    }

    [Fact]
    public async Task Me_returns_the_logged_in_persona_with_role_and_branch()
    {
        using var client = NewClient();
        await LoginAs(client, "manager-b2");

        var response = await client.GetAsync("/api/v1/me");
        var me = await ReadJson(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("BranchManager", me.GetProperty("role").GetString());
        Assert.Equal("สาขา 2", me.GetProperty("branch").GetProperty("name").GetString());
    }

    [Fact]
    public async Task Me_without_a_session_returns_401_problem_details()
    {
        using var client = NewClient();

        var response = await client.GetAsync("/api/v1/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Theory]
    [InlineData("not-a-jwt")]
    // Valid shape, signed with a different key.
    [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIwMTkzIiwicm9sZSI6Ik93bmVyIn0.c2lnbmF0dXJlLWZyb20tYW5vdGhlci1rZXk")]
    public async Task Me_with_a_forged_session_returns_401(string token)
    {
        using var client = NewClient(handleCookies: false);
        client.DefaultRequestHeaders.Add("Cookie", $"sf_session={token}");

        var response = await client.GetAsync("/api/v1/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Logout_ends_the_session()
    {
        using var client = NewClient();
        await LoginAs(client, "owner");

        var logout = await client.PostAsync("/api/v1/auth/logout", content: null);
        var me = await client.GetAsync("/api/v1/me");

        Assert.Equal(HttpStatusCode.NoContent, logout.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, me.StatusCode);
    }

    [Fact]
    public async Task Demo_login_with_an_unknown_persona_returns_404()
    {
        using var client = NewClient();

        var response = await client.PostAsJsonAsync("/api/v1/auth/demo-login", new { personaId = Guid.CreateVersion7() });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Demo_endpoints_do_not_exist_when_demo_mode_is_off()
    {
        await using var factory = db.CreateFactory(settings: new Dictionary<string, string?> { ["DEMO_MODE"] = "false" });
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost") });

        var personas = await client.GetAsync("/api/v1/auth/personas");
        var login = await client.PostAsJsonAsync("/api/v1/auth/demo-login", new { personaId = Guid.CreateVersion7() });

        Assert.Equal(HttpStatusCode.NotFound, personas.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, login.StatusCode);
    }

    private static async Task<List<JsonElement>> GetPersonas(HttpClient client)
    {
        var response = await client.GetAsync("/api/v1/auth/personas");
        response.EnsureSuccessStatusCode();
        return (await ReadJson(response)).EnumerateArray().ToList();
    }

    private static async Task<Guid> PersonaId(HttpClient client, string key) =>
        (await GetPersonas(client)).Single(p => p.GetProperty("key").GetString() == key).GetProperty("id").GetGuid();

    private static async Task LoginAs(HttpClient client, string key)
    {
        var response = await client.PostAsJsonAsync("/api/v1/auth/demo-login", new { personaId = await PersonaId(client, key) });
        response.EnsureSuccessStatusCode();
    }

    private static async Task<JsonElement> ReadJson(HttpResponseMessage response) =>
        JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
}
