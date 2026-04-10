using ChurchMS.Application.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(IUserService userService)
    : IRequestHandler<AssignUserRoleCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(
        AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var success = await userService.AssignRoleAsync(
            request.UserId, request.NewRole, request.CurrentRole);

        return success
            ? ApiResponse<bool>.SuccessResult(true, $"Role '{request.NewRole}' assigned.")
            : ApiResponse<bool>.FailureResult("User not found or role assignment failed.");
    }
}
