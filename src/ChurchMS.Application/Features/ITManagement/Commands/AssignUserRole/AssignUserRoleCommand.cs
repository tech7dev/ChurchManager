using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.AssignUserRole;

public record AssignUserRoleCommand(
    Guid UserId,
    string NewRole,
    string? CurrentRole
) : IRequest<ApiResponse<bool>>;
