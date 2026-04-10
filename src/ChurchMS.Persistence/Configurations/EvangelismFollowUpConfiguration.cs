using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class EvangelismFollowUpConfiguration : IEntityTypeConfiguration<EvangelismFollowUp>
{
    public void Configure(EntityTypeBuilder<EvangelismFollowUp> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Method).HasConversion<string>().HasMaxLength(30);
        builder.Property(f => f.Notes).HasMaxLength(2000);

        builder.HasIndex(f => f.ChurchId);
        builder.HasIndex(f => f.ContactId);

        builder.HasOne(f => f.Contact)
            .WithMany(c => c.FollowUps)
            .HasForeignKey(f => f.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.ConductedBy)
            .WithMany()
            .HasForeignKey(f => f.ConductedByMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
