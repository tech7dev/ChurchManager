using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Church-defined custom field that can be attached to members.
/// </summary>
public class CustomField : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Label { get; set; }
    public CustomFieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public string? Options { get; set; } // JSON array for Dropdown/MultiSelect types
    public string? DefaultValue { get; set; }
    public ICollection<MemberCustomFieldValue> Values { get; set; } = new List<MemberCustomFieldValue>();
}
