using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class EvangelismCampaignConfiguration : IEntityTypeConfiguration<EvangelismCampaign>
{
    public void Configure(EntityTypeBuilder<EvangelismCampaign> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.Notes).HasMaxLength(2000);

        builder.HasIndex(c => c.ChurchId);
        builder.HasIndex(c => c.Status);

        builder.HasOne(c => c.Leader)
            .WithMany()
            .HasForeignKey(c => c.LeaderMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
