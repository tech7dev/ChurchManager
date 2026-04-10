using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class ChurchConfiguration : IEntityTypeConfiguration<Church>
{
    public void Configure(EntityTypeBuilder<Church> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.Code)
            .IsUnique();

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.Property(c => c.LogoUrl)
            .HasMaxLength(500);

        builder.Property(c => c.Address)
            .HasMaxLength(500);

        builder.Property(c => c.City)
            .HasMaxLength(100);

        builder.Property(c => c.State)
            .HasMaxLength(100);

        builder.Property(c => c.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .HasMaxLength(30);

        builder.Property(c => c.Email)
            .HasMaxLength(200);

        builder.Property(c => c.Website)
            .HasMaxLength(300);

        builder.Property(c => c.TimeZone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.PrimaryCurrency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(c => c.SecondaryCurrency)
            .HasMaxLength(3);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(c => c.SubscriptionPlan)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        // Self-referencing hierarchy
        builder.HasOne(c => c.ParentChurch)
            .WithMany(c => c.ChildChurches)
            .HasForeignKey(c => c.ParentChurchId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.ParentChurchId);
    }
}
