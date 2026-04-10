using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.SetMemberCustomFieldValue;

public class SetMemberCustomFieldValueCommand : IRequest<ApiResponse<bool>>
{
    public Guid MemberId { get; set; }
    public Guid CustomFieldId { get; set; }
    public string? Value { get; set; }
}
