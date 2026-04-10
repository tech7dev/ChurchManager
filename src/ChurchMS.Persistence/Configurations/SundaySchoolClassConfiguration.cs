using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class SundaySchoolClassConfiguration : IEntityTypeConfiguration<SundaySchoolClass>
{
    public void Configure(EntityTypeBuilder<SundaySchoolClass> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.Level).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(c => c.ChurchId);

        builder.HasOne(c => c.Teacher)
            .WithMany()
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
