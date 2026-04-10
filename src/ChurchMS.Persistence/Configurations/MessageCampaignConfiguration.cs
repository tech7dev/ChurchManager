using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class MessageCampaignConfiguration : IEntityTypeConfiguration<MessageCampaign>
{
    public void Configure(EntityTypeBuilder<MessageCampaign> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Body).IsRequired().HasMaxLength(4000);
        builder.Property(c => c.Channel).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(c => c.ChurchId);
        builder.HasIndex(c => c.Status);

        builder.HasOne(c => c.SentBy)
            .WithMany()
            .HasForeignKey(c => c.SentByMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
