using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StockScribe.Api.Domain;
using StockScribe.Api.Features.Auth;
using StockScribe.Api.Infrastructure.Persistence;

namespace StockScribe.Api.Seed;

/// <summary>
/// Inserts the demo branches and personas that are missing. Safe to run on every start:
/// branches match on <see cref="Branch.Code"/>, personas on <see cref="User.PersonaKey"/>.
/// Later slices add suppliers, products, POs and lots here (docs/sa/seed-data.md).
/// </summary>
public sealed class DemoSeeder(AppDbContext db, ILogger<DemoSeeder> logger)
{
    public async Task SeedAsync(CancellationToken ct = default)
    {
        var codes = DemoPersonas.Branches.Select(b => b.Code).ToList();
        var branches = await db.Branches.Where(b => codes.Contains(b.Code)).ToDictionaryAsync(b => b.Code, ct);
        foreach (var (code, name) in DemoPersonas.Branches)
        {
            if (branches.ContainsKey(code)) continue;
            var branch = new Branch(code, name);
            db.Branches.Add(branch);
            branches[code] = branch;
        }

        var existingKeys = await db.Users
            .Where(u => u.PersonaKey != null)
            .Select(u => u.PersonaKey!)
            .ToListAsync(ct);
        foreach (var p in DemoPersonas.All.Where(p => !existingKeys.Contains(p.Key)))
        {
            var branchId = p.BranchCode is null ? (Guid?)null : branches[p.BranchCode].Id;
            db.Users.Add(User.CreatePersona(p.Key, p.Name, p.Role, branchId));
        }

        var added = await db.SaveChangesAsync(ct);
        logger.LogInformation("Demo seed finished, {Added} rows added", added);
    }
}

/// <summary>Runs <see cref="DemoSeeder"/> at start-up when SEED_DEMO=true (migrations must already be applied).</summary>
internal sealed class DemoSeedHostedService(IServiceScopeFactory scopes, IOptions<DemoOptions> demo) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        if (!demo.Value.Seed) return;
        await using var scope = scopes.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<DemoSeeder>().SeedAsync(ct);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
