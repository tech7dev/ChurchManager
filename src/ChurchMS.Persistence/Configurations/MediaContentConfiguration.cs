using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class MediaContentConfiguration : IEntityTypeConfiguration<MediaContent>
{
    public void Configure(EntityTypeBuilder<MediaContent> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title).IsRequired().HasMaxLength(300);
        builder.Property(c => c.Description).HasMaxLength(2000);
        builder.Property(c => c.ContentType).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.AccessType).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.Price).HasColumnType("decimal(18,2)");
        builder.Property(c => c.FileUrl).HasMaxLength(1000);
        builder.Property(c => c.ThumbnailUrl).HasMaxLength(1000);
        builder.Property(c => c.Tags).HasMaxLength(500);

        builder.HasIndex(c => c.ChurchId);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.ContentType);

        builder.HasOne(c => c.Author)
            .WithMany()
            .HasForeignKey(c => c.AuthorMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
