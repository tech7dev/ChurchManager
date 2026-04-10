using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class ChurchEventConfiguration : IEntityTypeConfiguration<ChurchEvent>
{
    public void Configure(EntityTypeBuilder<ChurchEvent> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(2000);
        builder.Property(e => e.Location).HasMaxLength(500);
        builder.Property(e => e.OnlineLink).HasMaxLength(500);
        builder.Property(e => e.Currency).HasMaxLength(3);
        builder.Property(e => e.QrCodeValue).HasMaxLength(200);
        builder.Property(e => e.RegistrationFee).HasColumnType("decimal(18,2)");
        builder.Property(e => e.Type).HasConversion<string>().HasMaxLength(20);
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(e => e.RecurrenceFrequency).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(e => e.ChurchId);
        builder.HasIndex(e => e.StartDateTime);
        builder.HasIndex(e => e.Status);
    }
}
