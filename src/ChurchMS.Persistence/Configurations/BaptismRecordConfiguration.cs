using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class BaptismRecordConfiguration : IEntityTypeConfiguration<BaptismRecord>
{
    public void Configure(EntityTypeBuilder<BaptismRecord> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Location).HasMaxLength(300);
        builder.Property(b => b.Notes).HasMaxLength(2000);

        builder.HasIndex(b => b.ChurchId);
        builder.HasIndex(b => b.MemberId);

        builder.HasOne(b => b.Member)
            .WithMany()
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Officiant)
            .WithMany()
            .HasForeignKey(b => b.OfficiantMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Certificate)
            .WithMany()
            .HasForeignKey(b => b.CertificateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
