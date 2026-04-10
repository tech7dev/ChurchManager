using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class MediaPromotionConfiguration : IEntityTypeConfiguration<MediaPromotion>
{
    public void Configure(EntityTypeBuilder<MediaPromotion> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.Code).HasMaxLength(50);
        builder.Property(p => p.DiscountPercent).HasColumnType("decimal(5,2)");
        builder.Property(p => p.DiscountAmount).HasColumnType("decimal(18,2)");

        builder.HasIndex(p => p.ChurchId);
        builder.HasIndex(p => p.IsActive);
        builder.HasIndex(p => p.Code).IsUnique().HasFilter("[Code] IS NOT NULL");

        builder.HasOne(p => p.Content)
            .WithMany(c => c.Promotions)
            .HasForeignKey(p => p.ContentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
