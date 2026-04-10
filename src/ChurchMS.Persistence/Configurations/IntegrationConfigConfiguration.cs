using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class IntegrationConfigConfiguration : IEntityTypeConfiguration<IntegrationConfig>
{
    public void Configure(EntityTypeBuilder<IntegrationConfig> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Service).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.ApiKey).HasMaxLength(500);
        builder.Property(c => c.ApiSecret).HasMaxLength(500);
        builder.Property(c => c.WebhookUrl).HasMaxLength(500);
        builder.Property(c => c.AdditionalConfig).HasMaxLength(4000);
        builder.Property(c => c.LastTestResult).HasMaxLength(1000);

        builder.HasIndex(c => c.ChurchId);
        builder.HasIndex(c => new { c.ChurchId, c.Service }).IsUnique();
    }
}
