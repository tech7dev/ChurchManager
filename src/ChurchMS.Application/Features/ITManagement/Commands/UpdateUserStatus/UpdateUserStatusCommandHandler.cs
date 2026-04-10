using ChurchMS.Application.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.UpdateUserStatus;

public class UpdateUserStatusCommandHandler(IUserService userService)
    : IRequestHandler<UpdateUserStatusCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(
        UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        var success = await userService.SetActiveStatusAsync(request.UserId, request.IsActive);

        return success
            ? ApiResponse<bool>.SuccessResult(true,
                request.IsActive ? "User activated." : "User deactivated.")
            : ApiResponse<bool>.FailureResult("User not found or update failed.");
    }
}
