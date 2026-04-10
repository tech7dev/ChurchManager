using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class ContributionCampaignConfiguration : IEntityTypeConfiguration<ContributionCampaign>
{
    public void Configure(EntityTypeBuilder<ContributionCampaign> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.TargetAmount).HasColumnType("decimal(18,2)");
        builder.Property(c => c.Currency).IsRequired().HasMaxLength(3);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(c => c.ChurchId);
        builder.HasIndex(c => c.Status);

        builder.HasOne(c => c.Fund)
            .WithMany()
            .HasForeignKey(c => c.FundId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
