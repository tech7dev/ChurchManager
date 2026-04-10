using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.CreateCustomField;

public class CreateCustomFieldCommand : IRequest<ApiResponse<CustomFieldDto>>
{
    public string Name { get; set; } = string.Empty;
    public string? Label { get; set; }
    public CustomFieldType FieldType { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public string? Options { get; set; }
    public string? DefaultValue { get; set; }
}
