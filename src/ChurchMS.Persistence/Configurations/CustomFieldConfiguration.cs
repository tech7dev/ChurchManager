using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class CustomFieldConfiguration : IEntityTypeConfiguration<CustomField>
{
    public void Configure(EntityTypeBuilder<CustomField> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Name).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Label).HasMaxLength(200);
        builder.Property(f => f.Options).HasMaxLength(2000);
        builder.Property(f => f.DefaultValue).HasMaxLength(500);
        builder.Property(f => f.FieldType).HasConversion<string>().HasMaxLength(20);
        builder.HasIndex(f => f.ChurchId);
    }
}
