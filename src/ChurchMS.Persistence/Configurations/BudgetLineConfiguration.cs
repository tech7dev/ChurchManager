using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class BudgetLineConfiguration : IEntityTypeConfiguration<BudgetLine>
{
    public void Configure(EntityTypeBuilder<BudgetLine> builder)
    {
        builder.HasKey(bl => bl.Id);
        builder.Property(bl => bl.Name).IsRequired().HasMaxLength(200);
        builder.Property(bl => bl.AllocatedAmount).HasColumnType("decimal(18,2)");
        builder.Property(bl => bl.SpentAmount).HasColumnType("decimal(18,2)");
        builder.Property(bl => bl.Notes).HasMaxLength(500);

        builder.HasIndex(bl => bl.ChurchId);
        builder.HasIndex(bl => bl.BudgetId);

        builder.HasOne(bl => bl.Budget)
            .WithMany(b => b.Lines)
            .HasForeignKey(bl => bl.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bl => bl.Category)
            .WithMany()
            .HasForeignKey(bl => bl.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
