using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class VehicleBookingConfiguration : IEntityTypeConfiguration<VehicleBooking>
{
    public void Configure(EntityTypeBuilder<VehicleBooking> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Purpose).IsRequired().HasMaxLength(300);
        builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(b => b.Notes).HasMaxLength(2000);

        builder.HasIndex(b => b.ChurchId);
        builder.HasIndex(b => b.VehicleId);
        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.StartDateTime);

        builder.HasOne(b => b.Vehicle)
            .WithMany(v => v.Bookings)
            .HasForeignKey(b => b.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Driver)
            .WithMany()
            .HasForeignKey(b => b.DriverMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.RelatedEvent)
            .WithMany()
            .HasForeignKey(b => b.RelatedEventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.ApprovedBy)
            .WithMany()
            .HasForeignKey(b => b.ApprovedByMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
