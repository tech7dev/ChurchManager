using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(t => t.Notes).HasMaxLength(2000);

        builder.HasIndex(t => t.ChurchId);
        builder.HasIndex(t => t.ItemId);
        builder.HasIndex(t => t.TransactionDate);

        builder.HasOne(t => t.Item)
            .WithMany(i => i.Transactions)
            .HasForeignKey(t => t.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.RelatedEvent)
            .WithMany()
            .HasForeignKey(t => t.RelatedEventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.RecordedBy)
            .WithMany()
            .HasForeignKey(t => t.RecordedByMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
