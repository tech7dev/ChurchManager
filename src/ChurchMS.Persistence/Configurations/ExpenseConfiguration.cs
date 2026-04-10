using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.Currency).IsRequired().HasMaxLength(3);
        builder.Property(e => e.ReceiptUrl).HasMaxLength(500);
        builder.Property(e => e.VendorName).HasMaxLength(200);
        builder.Property(e => e.RejectionReason).HasMaxLength(500);
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(e => e.PaymentMethod).HasConversion<string>().HasMaxLength(30);

        builder.HasIndex(e => e.ChurchId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.ExpenseDate);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.BankAccount)
            .WithMany()
            .HasForeignKey(e => e.BankAccountId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasOne(e => e.BudgetLine)
            .WithMany(bl => bl.Expenses)
            .HasForeignKey(e => e.BudgetLineId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
