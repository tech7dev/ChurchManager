using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Members.DTOs;

public class CustomFieldDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Label { get; set; }
    public CustomFieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public string? Options { get; set; }
    public string? DefaultValue { get; set; }
}
