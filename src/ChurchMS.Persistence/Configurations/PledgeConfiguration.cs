using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class PledgeConfiguration : IEntityTypeConfiguration<Pledge>
{
    public void Configure(EntityTypeBuilder<Pledge> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.PledgedAmount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.PaidAmount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.Currency).IsRequired().HasMaxLength(3);
        builder.Property(p => p.Notes).HasMaxLength(1000);
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(p => p.ChurchId);
        builder.HasIndex(p => p.MemberId);

        builder.HasOne(p => p.Member)
            .WithMany()
            .HasForeignKey(p => p.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Fund)
            .WithMany()
            .HasForeignKey(p => p.FundId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Campaign)
            .WithMany()
            .HasForeignKey(p => p.CampaignId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
