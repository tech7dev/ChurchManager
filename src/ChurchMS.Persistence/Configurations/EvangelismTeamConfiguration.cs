using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class EvangelismTeamConfiguration : IEntityTypeConfiguration<EvangelismTeam>
{
    public void Configure(EntityTypeBuilder<EvangelismTeam> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Notes).HasMaxLength(2000);

        builder.HasIndex(t => t.ChurchId);
        builder.HasIndex(t => t.CampaignId);

        builder.HasOne(t => t.Campaign)
            .WithMany(c => c.Teams)
            .HasForeignKey(t => t.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Leader)
            .WithMany()
            .HasForeignKey(t => t.LeaderMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
