using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class DepartmentMemberConfiguration : IEntityTypeConfiguration<DepartmentMember>
{
    public void Configure(EntityTypeBuilder<DepartmentMember> builder)
    {
        builder.HasKey(dm => dm.Id);
        builder.Property(dm => dm.Role).HasConversion<string>().HasMaxLength(30);
        builder.Property(dm => dm.Notes).HasMaxLength(500);

        builder.HasIndex(dm => dm.ChurchId);
        builder.HasIndex(dm => dm.DepartmentId);
        builder.HasIndex(dm => dm.MemberId);
        builder.HasIndex(dm => new { dm.DepartmentId, dm.MemberId });

        builder.HasOne(dm => dm.Department)
            .WithMany(d => d.Members)
            .HasForeignKey(dm => dm.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(dm => dm.Member)
            .WithMany()
            .HasForeignKey(dm => dm.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
