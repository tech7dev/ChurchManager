using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class EvangelismContactConfiguration : IEntityTypeConfiguration<EvangelismContact>
{
    public void Configure(EntityTypeBuilder<EvangelismContact> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Phone).HasMaxLength(30);
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.Address).HasMaxLength(500);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.Notes).HasMaxLength(2000);

        builder.HasIndex(c => c.ChurchId);
        builder.HasIndex(c => c.CampaignId);
        builder.HasIndex(c => c.Status);

        builder.HasOne(c => c.Campaign)
            .WithMany(camp => camp.Contacts)
            .HasForeignKey(c => c.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Team)
            .WithMany()
            .HasForeignKey(c => c.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.AssignedTo)
            .WithMany()
            .HasForeignKey(c => c.AssignedToMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.ConvertedMember)
            .WithMany()
            .HasForeignKey(c => c.ConvertedMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
