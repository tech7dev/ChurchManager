using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class VisitorConfiguration : IEntityTypeConfiguration<Visitor>
{
    public void Configure(EntityTypeBuilder<Visitor> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(v => v.LastName).IsRequired().HasMaxLength(100);
        builder.Property(v => v.Phone).HasMaxLength(30);
        builder.Property(v => v.Email).HasMaxLength(200);
        builder.Property(v => v.Address).HasMaxLength(500);
        builder.Property(v => v.HowHeardAboutUs).HasMaxLength(500);
        builder.Property(v => v.Notes).HasMaxLength(2000);
        builder.Property(v => v.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(v => v.Gender).HasConversion<string>().HasMaxLength(10);

        builder.HasIndex(v => v.ChurchId);
        builder.HasIndex(v => v.Status);

        builder.HasOne(v => v.ConvertedToMember)
            .WithMany()
            .HasForeignKey(v => v.ConvertedToMemberId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasOne(v => v.AssignedToMember)
            .WithMany()
            .HasForeignKey(v => v.AssignedToMemberId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
