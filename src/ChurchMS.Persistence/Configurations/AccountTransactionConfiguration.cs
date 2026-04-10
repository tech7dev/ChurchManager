using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class AccountTransactionConfiguration : IEntityTypeConfiguration<AccountTransaction>
{
    public void Configure(EntityTypeBuilder<AccountTransaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Amount).HasColumnType("decimal(18,2)");
        builder.Property(t => t.Currency).IsRequired().HasMaxLength(3);
        builder.Property(t => t.Description).IsRequired().HasMaxLength(500);
        builder.Property(t => t.Reference).HasMaxLength(200);
        builder.Property(t => t.RunningBalance).HasColumnType("decimal(18,2)");
        builder.Property(t => t.Type).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(t => t.ChurchId);
        builder.HasIndex(t => t.BankAccountId);
        builder.HasIndex(t => t.TransactionDate);

        builder.HasOne(t => t.BankAccount)
            .WithMany(b => b.Transactions)
            .HasForeignKey(t => t.BankAccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
