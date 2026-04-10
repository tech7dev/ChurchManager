using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class MarriageRecordConfiguration : IEntityTypeConfiguration<MarriageRecord>
{
    public void Configure(EntityTypeBuilder<MarriageRecord> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Spouse2Name).HasMaxLength(200);
        builder.Property(m => m.Spouse2Phone).HasMaxLength(30);
        builder.Property(m => m.Location).HasMaxLength(300);
        builder.Property(m => m.Notes).HasMaxLength(2000);

        builder.HasIndex(m => m.ChurchId);
        builder.HasIndex(m => m.Spouse1MemberId);
        builder.HasIndex(m => m.Spouse2MemberId);

        builder.HasOne(m => m.Spouse1)
            .WithMany()
            .HasForeignKey(m => m.Spouse1MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Spouse2)
            .WithMany()
            .HasForeignKey(m => m.Spouse2MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Officiant)
            .WithMany()
            .HasForeignKey(m => m.OfficiantMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Certificate)
            .WithMany()
            .HasForeignKey(m => m.CertificateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
