using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CertificateNumber).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.FileUrl).HasMaxLength(1000);
        builder.Property(c => c.Notes).HasMaxLength(2000);

        builder.HasIndex(c => c.ChurchId);
        builder.HasIndex(c => c.MemberId);
        builder.HasIndex(c => new { c.ChurchId, c.CertificateNumber }).IsUnique();

        builder.HasOne(c => c.Member)
            .WithMany()
            .HasForeignKey(c => c.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.IssuedBy)
            .WithMany()
            .HasForeignKey(c => c.IssuedByMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
