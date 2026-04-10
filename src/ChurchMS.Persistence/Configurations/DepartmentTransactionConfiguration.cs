using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class DepartmentTransactionConfiguration : IEntityTypeConfiguration<DepartmentTransaction>
{
    public void Configure(EntityTypeBuilder<DepartmentTransaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Type).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.Amount).HasColumnType("decimal(18,2)");
        builder.Property(t => t.Description).IsRequired().HasMaxLength(500);
        builder.Property(t => t.ReferenceNumber).HasMaxLength(100);

        builder.HasIndex(t => t.ChurchId);
        builder.HasIndex(t => t.DepartmentId);
        builder.HasIndex(t => t.Date);

        builder.HasOne(t => t.Department)
            .WithMany(d => d.Transactions)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.RecordedBy)
            .WithMany()
            .HasForeignKey(t => t.RecordedByMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
