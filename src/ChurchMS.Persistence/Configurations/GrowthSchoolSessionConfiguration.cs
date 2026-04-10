using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class GrowthSchoolSessionConfiguration : IEntityTypeConfiguration<GrowthSchoolSession>
{
    public void Configure(EntityTypeBuilder<GrowthSchoolSession> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Title).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Description).HasMaxLength(1000);
        builder.Property(s => s.SessionNotes).HasMaxLength(4000);

        builder.HasIndex(s => s.ChurchId);
        builder.HasIndex(s => s.CourseId);
        builder.HasIndex(s => s.SessionDate);

        builder.HasOne(s => s.Course)
            .WithMany(c => c.Sessions)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
