using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class SundaySchoolAttendanceConfiguration : IEntityTypeConfiguration<SundaySchoolAttendance>
{
    public void Configure(EntityTypeBuilder<SundaySchoolAttendance> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Notes).HasMaxLength(500);
        builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(a => a.ChurchId);
        builder.HasIndex(a => a.LessonId);

        builder.HasOne(a => a.Lesson)
            .WithMany(l => l.AttendanceRecords)
            .HasForeignKey(a => a.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Member)
            .WithMany()
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
