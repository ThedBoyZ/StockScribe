using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockScribe.Api.Domain;

namespace StockScribe.Api.Infrastructure.Persistence.Configurations;

internal sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> b)
    {
        b.ToTable("branches");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever(); // UUIDv7 assigned in the domain

        b.Property(x => x.Code).HasMaxLength(Branch.CodeMaxLength).IsRequired();
        b.HasIndex(x => x.Code).IsUnique();

        b.Property(x => x.Name).HasMaxLength(Branch.NameMaxLength).IsRequired();
    }
}
