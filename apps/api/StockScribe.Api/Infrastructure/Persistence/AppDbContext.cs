using Microsoft.EntityFrameworkCore;
using StockScribe.Api.Domain;

namespace StockScribe.Api.Infrastructure.Persistence;

/// <summary>
/// PostgreSQL (transactional data, ADR-0001). Names are snake_case via EFCore.NamingConventions;
/// mappings live in Configurations/.
/// </summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
