using ChurchMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchMS.Persistence.Configurations;

public class MemberCustomFieldValueConfiguration : IEntityTypeConfiguration<MemberCustomFieldValue>
{
    public void Configure(EntityTypeBuilder<MemberCustomFieldValue> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Value).HasMaxLength(2000);
        builder.HasIndex(v => new { v.MemberId, v.CustomFieldId }).IsUnique();
        builder.HasIndex(v => v.ChurchId);

        builder.HasOne(v => v.CustomField)
            .WithMany(f => f.Values)
            .HasForeignKey(v => v.CustomFieldId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
