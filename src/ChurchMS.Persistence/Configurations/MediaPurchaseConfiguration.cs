using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class MediaPurchaseConfiguration : IEntityTypeConfiguration<MediaPurchase>
{
    public void Configure(EntityTypeBuilder<MediaPurchase> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(p => p.PaymentReference).HasMaxLength(200);

        builder.HasIndex(p => p.ChurchId);
        builder.HasIndex(p => p.ContentId);
        builder.HasIndex(p => p.MemberId);
        builder.HasIndex(p => p.Status);

        builder.HasOne(p => p.Content)
            .WithMany(c => c.Purchases)
            .HasForeignKey(p => p.ContentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Member)
            .WithMany()
            .HasForeignKey(p => p.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.ActivatedBy)
            .WithMany()
            .HasForeignKey(p => p.ActivatedByMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
