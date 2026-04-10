using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class SystemLogConfiguration : IEntityTypeConfiguration<SystemLog>
{
    public void Configure(EntityTypeBuilder<SystemLog> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Action).IsRequired().HasMaxLength(200);
        builder.Property(l => l.EntityType).HasMaxLength(100);
        builder.Property(l => l.Details).HasMaxLength(4000);
        builder.Property(l => l.IpAddress).HasMaxLength(45);
        builder.Property(l => l.UserAgent).HasMaxLength(500);
        builder.Property(l => l.Level).IsRequired().HasMaxLength(20);

        builder.HasIndex(l => l.ChurchId);
        builder.HasIndex(l => l.CreatedAt);
        builder.HasIndex(l => l.Level);
        builder.HasIndex(l => l.UserId);
    }
}
