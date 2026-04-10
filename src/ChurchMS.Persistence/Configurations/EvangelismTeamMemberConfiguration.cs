using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class EvangelismTeamMemberConfiguration : IEntityTypeConfiguration<EvangelismTeamMember>
{
    public void Configure(EntityTypeBuilder<EvangelismTeamMember> builder)
    {
        builder.HasKey(tm => tm.Id);

        builder.HasIndex(tm => tm.ChurchId);
        builder.HasIndex(tm => new { tm.TeamId, tm.MemberId }).IsUnique();

        builder.HasOne(tm => tm.Team)
            .WithMany(t => t.Members)
            .HasForeignKey(tm => tm.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tm => tm.Member)
            .WithMany()
            .HasForeignKey(tm => tm.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
