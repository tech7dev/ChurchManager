using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Commands.AssignMember;

public record AssignMemberCommand(
    Guid DepartmentId,
    Guid MemberId,
    DepartmentMemberRole Role,
    DateOnly JoinedDate,
    string? Notes
) : IRequest<ApiResponse<DepartmentMemberDto>>;
