using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class ContributionConfiguration : IEntityTypeConfiguration<Contribution>
{
    public void Configure(EntityTypeBuilder<Contribution> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.ReferenceNumber).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Amount).HasColumnType("decimal(18,2)");
        builder.Property(c => c.Currency).IsRequired().HasMaxLength(3);
        builder.Property(c => c.Notes).HasMaxLength(1000);
        builder.Property(c => c.CheckNumber).HasMaxLength(50);
        builder.Property(c => c.TransactionReference).HasMaxLength(200);
        builder.Property(c => c.AnonymousDonorName).HasMaxLength(200);
        builder.Property(c => c.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.RecurrenceFrequency).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(c => c.ChurchId);
        builder.HasIndex(c => c.ContributionDate);
        builder.HasIndex(c => c.ReferenceNumber).IsUnique();

        builder.HasOne(c => c.Fund)
            .WithMany(f => f.Contributions)
            .HasForeignKey(c => c.FundId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Campaign)
            .WithMany(ca => ca.Contributions)
            .HasForeignKey(c => c.CampaignId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasOne(c => c.Member)
            .WithMany()
            .HasForeignKey(c => c.MemberId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
