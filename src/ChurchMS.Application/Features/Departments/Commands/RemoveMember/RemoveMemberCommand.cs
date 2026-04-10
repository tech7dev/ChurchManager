using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Commands.RemoveMember;

public record RemoveMemberCommand(
    Guid DepartmentMemberId,
    DateOnly? LeftDate
) : IRequest<ApiResponse<bool>>;
