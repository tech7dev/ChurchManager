using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Make).IsRequired().HasMaxLength(100);
        builder.Property(v => v.Model).IsRequired().HasMaxLength(100);
        builder.Property(v => v.PlateNumber).HasMaxLength(20);
        builder.Property(v => v.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(v => v.Color).HasMaxLength(50);
        builder.Property(v => v.Notes).HasMaxLength(2000);

        builder.HasIndex(v => v.ChurchId);
        builder.HasIndex(v => v.Status);
        builder.HasIndex(v => v.PlateNumber).IsUnique().HasFilter("[PlateNumber] IS NOT NULL");
    }
}
