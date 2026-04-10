using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class SmsCreditConfiguration : IEntityTypeConfiguration<SmsCredit>
{
    public void Configure(EntityTypeBuilder<SmsCredit> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasIndex(s => s.ChurchId).IsUnique();

        builder.HasMany(s => s.Transactions)
            .WithOne(t => t.SmsCredit)
            .HasForeignKey(t => t.SmsCreditId);
    }
}
