using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(m => m.MiddleName).HasMaxLength(100);
        builder.Property(m => m.LastName).IsRequired().HasMaxLength(100);
        builder.Property(m => m.MembershipNumber).IsRequired().HasMaxLength(30);
        builder.Property(m => m.PhotoUrl).HasMaxLength(500);
        builder.Property(m => m.NationalId).HasMaxLength(50);
        builder.Property(m => m.Phone).HasMaxLength(30);
        builder.Property(m => m.AlternatePhone).HasMaxLength(30);
        builder.Property(m => m.Email).HasMaxLength(200);
        builder.Property(m => m.Address).HasMaxLength(500);
        builder.Property(m => m.City).HasMaxLength(100);
        builder.Property(m => m.State).HasMaxLength(100);
        builder.Property(m => m.Country).HasMaxLength(100);
        builder.Property(m => m.PostalCode).HasMaxLength(20);
        builder.Property(m => m.BaptizedBy).HasMaxLength(200);
        builder.Property(m => m.Notes).HasMaxLength(2000);
        builder.Property(m => m.Occupation).HasMaxLength(200);
        builder.Property(m => m.Employer).HasMaxLength(200);
        builder.Property(m => m.QrCodeValue).IsRequired().HasMaxLength(200);

        builder.Property(m => m.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(m => m.Gender).HasConversion<string>().HasMaxLength(10);
        builder.Property(m => m.MaritalStatus).HasConversion<string>().HasMaxLength(20);
        builder.Property(m => m.BloodType).HasConversion<string>().HasMaxLength(15);
        builder.Property(m => m.FamilyRole).HasConversion<string>().HasMaxLength(20);

        // Composite unique index: membership number per church
        builder.HasIndex(m => new { m.ChurchId, m.MembershipNumber }).IsUnique();
        builder.HasIndex(m => m.ChurchId);
        builder.HasIndex(m => m.Status);
        builder.HasIndex(m => m.QrCodeValue).IsUnique();

        builder.HasOne(m => m.Family)
            .WithMany(f => f.Members)
            .HasForeignKey(m => m.FamilyId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasMany(m => m.CustomFieldValues)
            .WithOne(v => v.Member)
            .HasForeignKey(v => v.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
