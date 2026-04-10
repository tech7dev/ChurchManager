using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Value of a custom field for a specific member.
/// </summary>
public class MemberCustomFieldValue : TenantEntity
{
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public Guid CustomFieldId { get; set; }
    public CustomField CustomField { get; set; } = null!;

    public string? Value { get; set; }
}
