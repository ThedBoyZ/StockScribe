using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockScribe.Api.Domain;

namespace StockScribe.Api.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public const string BranchRequiredCheck = "ck_users_branch_required_for_role";

    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("users", t => t.HasCheckConstraint(
            BranchRequiredCheck,
            "role NOT IN ('Staff', 'BranchManager') OR branch_id IS NOT NULL"));

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever(); // UUIDv7 assigned in the domain

        b.Property(x => x.DisplayName).HasMaxLength(User.DisplayNameMaxLength).IsRequired();
        b.Property(x => x.Role).HasConversion<string>().HasMaxLength(20).IsRequired();

        b.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
