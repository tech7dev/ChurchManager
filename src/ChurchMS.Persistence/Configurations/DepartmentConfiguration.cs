using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
        builder.Property(d => d.Description).HasMaxLength(1000);
        builder.Property(d => d.Color).HasMaxLength(7);

        builder.HasIndex(d => d.ChurchId);
        builder.HasIndex(d => new { d.ChurchId, d.Name });

        builder.HasOne(d => d.Leader)
            .WithMany()
            .HasForeignKey(d => d.LeaderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
