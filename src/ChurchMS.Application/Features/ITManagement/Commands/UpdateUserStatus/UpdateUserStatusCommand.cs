using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.UpdateUserStatus;

public record UpdateUserStatusCommand(
    Guid UserId,
    bool IsActive
) : IRequest<ApiResponse<bool>>;
