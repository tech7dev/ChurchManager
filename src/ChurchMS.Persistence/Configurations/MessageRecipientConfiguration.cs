using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class MessageRecipientConfiguration : IEntityTypeConfiguration<MessageRecipient>
{
    public void Configure(EntityTypeBuilder<MessageRecipient> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(r => r.GuestName).HasMaxLength(200);
        builder.Property(r => r.GuestPhone).HasMaxLength(30);
        builder.Property(r => r.GuestEmail).HasMaxLength(200);
        builder.Property(r => r.ErrorMessage).HasMaxLength(500);

        builder.HasIndex(r => r.ChurchId);
        builder.HasIndex(r => r.CampaignId);
        builder.HasIndex(r => r.MemberId);

        builder.HasOne(r => r.Campaign)
            .WithMany(c => c.Recipients)
            .HasForeignKey(r => r.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Member)
            .WithMany()
            .HasForeignKey(r => r.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
