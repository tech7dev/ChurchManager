using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class SundaySchoolLessonConfiguration : IEntityTypeConfiguration<SundaySchoolLesson>
{
    public void Configure(EntityTypeBuilder<SundaySchoolLesson> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Title).IsRequired().HasMaxLength(200);
        builder.Property(l => l.Description).HasMaxLength(1000);
        builder.Property(l => l.LessonNotes).HasMaxLength(4000);

        builder.HasIndex(l => l.ChurchId);
        builder.HasIndex(l => l.ClassId);
        builder.HasIndex(l => l.LessonDate);

        builder.HasOne(l => l.Class)
            .WithMany(c => c.Lessons)
            .HasForeignKey(l => l.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
