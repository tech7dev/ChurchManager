using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Subject).IsRequired().HasMaxLength(300);
        builder.Property(a => a.Description).HasMaxLength(2000);
        builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(a => a.MeetingType).HasConversion<string>().HasMaxLength(20);
        builder.Property(a => a.VideoCallLink).HasMaxLength(500);
        builder.Property(a => a.Location).HasMaxLength(300);
        builder.Property(a => a.Notes).HasMaxLength(2000);

        builder.HasIndex(a => a.ChurchId);
        builder.HasIndex(a => a.MemberId);
        builder.HasIndex(a => a.ResponsibleMemberId);
        builder.HasIndex(a => a.Status);
        builder.HasIndex(a => a.ScheduledAt);

        builder.HasOne(a => a.Member)
            .WithMany()
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Responsible)
            .WithMany()
            .HasForeignKey(a => a.ResponsibleMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
