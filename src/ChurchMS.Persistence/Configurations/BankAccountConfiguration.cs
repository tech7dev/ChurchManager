using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.AccountName).IsRequired().HasMaxLength(200);
        builder.Property(b => b.AccountNumber).IsRequired().HasMaxLength(50);
        builder.Property(b => b.BankName).IsRequired().HasMaxLength(200);
        builder.Property(b => b.BranchName).HasMaxLength(200);
        builder.Property(b => b.SwiftCode).HasMaxLength(20);
        builder.Property(b => b.RoutingNumber).HasMaxLength(30);
        builder.Property(b => b.Currency).IsRequired().HasMaxLength(3);
        builder.Property(b => b.CurrentBalance).HasColumnType("decimal(18,2)");
        builder.Property(b => b.AccountType).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(b => b.ChurchId);
    }
}
