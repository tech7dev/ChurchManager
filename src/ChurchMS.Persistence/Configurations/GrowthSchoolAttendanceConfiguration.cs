using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class GrowthSchoolAttendanceConfiguration : IEntityTypeConfiguration<GrowthSchoolAttendance>
{
    public void Configure(EntityTypeBuilder<GrowthSchoolAttendance> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Notes).HasMaxLength(500);
        builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(a => a.ChurchId);
        builder.HasIndex(a => a.SessionId);

        builder.HasOne(a => a.Session)
            .WithMany(s => s.AttendanceRecords)
            .HasForeignKey(a => a.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Member)
            .WithMany()
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
