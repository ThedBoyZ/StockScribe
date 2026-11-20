using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StockScribe.Api.Infrastructure.Mongo;
using StockScribe.Api.Seed;

namespace StockScribe.Tests.Integration;

/// <summary>US-002 through HTTP: the queue, the approval limit, 403 for direct API calls, 409 for stale versions.</summary>
[Collection(IntegrationCollection.Name)]
[Trait("Category", "Integration")]
public class PurchaseOrderApprovalEndpointTests(DatabaseFixture db) : IAsyncLifetime
{
    private WebApplicationFactory<Program> _factory = null!;

    public async Task InitializeAsync()
    {
        _factory = db.CreateFactory(settings: new Dictionary<string, string?> { ["DEMO_MODE"] = "true", ["SEED_DEMO"] = "true" });
        // Every test starts from the full demo queue.
        await using var scope = _factory.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<DemoSeeder>().ResetPurchaseOrdersAsync();
    }

    public async Task DisposeAsync() => await _factory.DisposeAsync();

    [Fact]
    public async Task Manager_queue_has_the_four_branch_2_pos_with_approval_info()
    {
        using var client = await LoginAs("manager-b2");

        var queue = await Queue(client);

        Assert.Equal(["PO-2026-0835", "PO-2026-0836", "PO-2026-0837", "PO-2026-0838"], queue.Select(p => p.GetProperty("number").GetString()));
        var po0835 = queue[0];
        Assert.Equal(4200.00m, po0835.GetProperty("subtotal").GetDecimal());
        Assert.Equal(4494.00m, po0835.GetProperty("total").GetDecimal());
        Assert.Equal(3, po0835.GetProperty("lines").GetArrayLength());
        Assert.True(po0835.GetProperty("approval").GetProperty("canApprove").GetBoolean());
        Assert.Equal(800.00m, po0835.GetProperty("approval").GetProperty("remaining").GetDecimal());

        var po0838 = queue[3].GetProperty("approval");
        Assert.False(po0838.GetProperty("canApprove").GetBoolean());
        Assert.Equal("Owner", po0838.GetProperty("requiredRole").GetString());
    }

    [Fact]
    public async Task Manager_approves_within_the_limit_and_it_is_audited()
    {
        using var client = await LoginAs("manager-b2");
        var po = (await Queue(client))[0];

        var response = await Decide(client, po, "approve");
        var body = await ReadJson(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Approved", body.GetProperty("status").GetString());
        Assert.Equal("นภา ใจดี", body.GetProperty("by").GetString());

        var audit = _factory.Services.GetRequiredService<AuditLog>().Collection;
        var entry = await audit.Find(e => e.EntityId == po.GetProperty("id").GetGuid() && e.Action == "PurchaseOrder.Approved")
            .SortByDescending(e => e.At).FirstOrDefaultAsync();
        Assert.NotNull(entry);
        Assert.Equal("BranchManager", entry.UserRole);
    }

    [Fact]
    public async Task Manager_calling_the_api_directly_above_the_limit_gets_403()
    {
        using var client = await LoginAs("manager-b2");
        var po0838 = (await Queue(client))[3];

        var response = await Decide(client, po0838, "approve");
        var body = await ReadJson(response);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.EndsWith("/approval-limit-exceeded", body.GetProperty("type").GetString());
    }

    [Fact]
    public async Task Owner_approves_above_the_limit()
    {
        using var client = await LoginAs("owner");
        var po0838 = (await Queue(client)).Single(p => p.GetProperty("number").GetString() == "PO-2026-0838");

        var response = await Decide(client, po0838, "approve");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Stale_version_returns_409_and_second_approval_is_rejected()
    {
        using var client = await LoginAs("manager-b2");
        var po = (await Queue(client))[1];

        var stale = await client.PostAsJsonAsync($"/api/v1/purchase-orders/{po.GetProperty("id").GetGuid()}/approve",
            new { version = po.GetProperty("version").GetUInt32() + 1 });
        Assert.Equal(HttpStatusCode.Conflict, stale.StatusCode);

        Assert.Equal(HttpStatusCode.OK, (await Decide(client, po, "approve")).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, (await Decide(client, po, "approve")).StatusCode); // old version
    }

    [Fact]
    public async Task Reject_without_reason_is_422_and_with_reason_returns_to_draft()
    {
        using var client = await LoginAs("manager-b2");
        var po = (await Queue(client))[2];

        Assert.Equal(HttpStatusCode.UnprocessableEntity, (await Decide(client, po, "reject", reason: "")).StatusCode);

        var response = await Decide(client, po, "reject", reason: "ราคาสูงกว่ารอบก่อน");
        Assert.Equal("Draft", (await ReadJson(response)).GetProperty("status").GetString());
    }

    [Fact]
    public async Task Staff_cannot_list_purchase_orders()
    {
        using var client = await LoginAs("staff-b2");

        var response = await client.GetAsync("/api/v1/purchase-orders?status=PendingApproval&mine=true");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    private async Task<HttpClient> LoginAs(string personaKey)
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost") });
        var personas = await ReadJson(await client.GetAsync("/api/v1/auth/personas"));
        var id = personas.EnumerateArray().Single(p => p.GetProperty("key").GetString() == personaKey).GetProperty("id").GetGuid();
        (await client.PostAsJsonAsync("/api/v1/auth/demo-login", new { personaId = id })).EnsureSuccessStatusCode();
        return client;
    }

    private static async Task<List<JsonElement>> Queue(HttpClient client)
    {
        var response = await client.GetAsync("/api/v1/purchase-orders?status=PendingApproval&mine=true");
        response.EnsureSuccessStatusCode();
        return (await ReadJson(response)).EnumerateArray().ToList();
    }

    private static Task<HttpResponseMessage> Decide(HttpClient client, JsonElement po, string action, string? reason = null) =>
        client.PostAsJsonAsync($"/api/v1/purchase-orders/{po.GetProperty("id").GetGuid()}/{action}",
            new { version = po.GetProperty("version").GetUInt32(), reason });

    private static async Task<JsonElement> ReadJson(HttpResponseMessage response) =>
        JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
}
