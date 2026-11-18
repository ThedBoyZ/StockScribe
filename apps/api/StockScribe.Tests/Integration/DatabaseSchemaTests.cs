using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using StockScribe.Api.Domain;
using StockScribe.Api.Infrastructure.Persistence;

namespace StockScribe.Tests.Integration;

/// <summary>Applies the real migrations, then checks the constraints the domain relies on.</summary>
[Collection(IntegrationCollection.Name)]
[Trait("Category", "Integration")]
public class DatabaseSchemaTests(DatabaseFixture db) : IAsyncLifetime
{
    private Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program> _factory = null!;

    public async Task InitializeAsync()
    {
        _factory = db.CreateFactory();
        await using var scope = _factory.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
    }

    public async Task DisposeAsync() => await _factory.DisposeAsync();

    [Fact]
    public async Task All_migrations_are_applied()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        Assert.Empty(await context.Database.GetPendingMigrationsAsync());
    }

    [Fact]
    public async Task Saves_and_reads_a_branch_user()
    {
        var branch = new Branch(UniqueCode(), "สาขาทดสอบ");
        var user = new User("พนักงานทดสอบ", UserRole.Staff, branch.Id, isDemo: true);

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.AddRange(branch, user);
            await context.SaveChangesAsync();
        }

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var saved = await context.Users.Include(u => u.Branch).SingleAsync(u => u.Id == user.Id);

            Assert.Equal(UserRole.Staff, saved.Role);
            Assert.Equal(branch.Code, saved.Branch!.Code);
            Assert.True(saved.IsDemo);
        }
    }

    [Fact]
    public async Task Database_rejects_staff_without_branch()
    {
        // Bypass the domain constructor to prove the CHECK constraint is a second line of defence.
        await using var scope = _factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var ex = await Assert.ThrowsAsync<PostgresException>(() => context.Database.ExecuteSqlRawAsync(
            "INSERT INTO users (id, display_name, role, branch_id, is_demo) VALUES ({0}, 'x', 'Staff', NULL, false)",
            Guid.CreateVersion7()));

        Assert.Equal(PostgresErrorCodes.CheckViolation, ex.SqlState);
        Assert.Equal("ck_users_branch_required_for_role", ex.ConstraintName);
    }

    [Fact]
    public async Task Branch_code_is_unique()
    {
        var code = UniqueCode();
        await using var scope = _factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Branches.Add(new Branch(code, "สาขา ก"));
        await context.SaveChangesAsync();
        context.Branches.Add(new Branch(code, "สาขา ข"));

        var ex = await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
        Assert.Equal(PostgresErrorCodes.UniqueViolation, Assert.IsType<PostgresException>(ex.InnerException).SqlState);
    }

    // Tests share one database, so each one uses its own branch code (max 10 chars).
    private static string UniqueCode() => "T" + Guid.NewGuid().ToString("N")[..8];
}
