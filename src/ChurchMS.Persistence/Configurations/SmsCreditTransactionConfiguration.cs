using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class SmsCreditTransactionConfiguration : IEntityTypeConfiguration<SmsCreditTransaction>
{
    public void Configure(EntityTypeBuilder<SmsCreditTransaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Type).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.Reference).HasMaxLength(200);
        builder.Property(t => t.Notes).HasMaxLength(500);

        builder.HasIndex(t => t.ChurchId);
        builder.HasIndex(t => t.SmsCreditId);
        builder.HasIndex(t => t.CreatedAt);
    }
}
