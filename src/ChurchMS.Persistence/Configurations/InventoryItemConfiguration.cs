using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name).IsRequired().HasMaxLength(200);
        builder.Property(i => i.Description).HasMaxLength(1000);
        builder.Property(i => i.Category).HasMaxLength(100);
        builder.Property(i => i.Unit).HasMaxLength(30);
        builder.Property(i => i.Location).HasMaxLength(200);
        builder.Property(i => i.SerialNumber).HasMaxLength(100);
        builder.Property(i => i.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(i => i.Notes).HasMaxLength(2000);

        builder.HasIndex(i => i.ChurchId);
        builder.HasIndex(i => i.Category);
        builder.HasIndex(i => i.Status);
    }
}
