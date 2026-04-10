using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class GrowthSchoolCourseConfiguration : IEntityTypeConfiguration<GrowthSchoolCourse>
{
    public void Configure(EntityTypeBuilder<GrowthSchoolCourse> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.Level).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(c => c.ChurchId);

        builder.HasOne(c => c.Instructor)
            .WithMany()
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
