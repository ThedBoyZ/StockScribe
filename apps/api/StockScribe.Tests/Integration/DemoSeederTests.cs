using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockScribe.Api.Infrastructure.Persistence;
using StockScribe.Api.Seed;

namespace StockScribe.Tests.Integration;

[Collection(IntegrationCollection.Name)]
[Trait("Category", "Integration")]
public class DemoSeederTests(DatabaseFixture db)
{
    [Fact]
    public async Task Seeding_again_adds_nothing()
    {
        await using var factory = db.CreateFactory();

        await Seed(factory);
        await Seed(factory);

        await using var scope = factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var codes = DemoPersonas.Branches.Select(b => b.Code).ToList();

        Assert.Equal(3, await context.Branches.CountAsync(b => codes.Contains(b.Code)));
        Assert.Equal(4, await context.Users.CountAsync(u => u.PersonaKey != null));
        // Staff and manager personas belong to สาขา 2.
        Assert.Equal(2, await context.Users.CountAsync(u => u.PersonaKey != null && u.Branch!.Code == "B2"));
    }

    private static async Task Seed(Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program> factory)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<DemoSeeder>().SeedAsync();
    }
}
