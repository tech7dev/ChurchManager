using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class EventRegistrationConfiguration : IEntityTypeConfiguration<EventRegistration>
{
    public void Configure(EntityTypeBuilder<EventRegistration> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.GuestName).HasMaxLength(200);
        builder.Property(r => r.GuestEmail).HasMaxLength(200);
        builder.Property(r => r.GuestPhone).HasMaxLength(30);
        builder.Property(r => r.RegistrationCode).HasMaxLength(50);
        builder.Property(r => r.Notes).HasMaxLength(500);
        builder.Property(r => r.AmountPaid).HasColumnType("decimal(18,2)");
        builder.Property(r => r.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(r => r.ChurchId);
        builder.HasIndex(r => r.EventId);
        builder.HasIndex(r => r.MemberId);

        builder.HasOne(r => r.Event)
            .WithMany(e => e.Registrations)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Member)
            .WithMany()
            .HasForeignKey(r => r.MemberId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
