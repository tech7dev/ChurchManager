using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserService userService)
    : IRequestHandler<GetUserByIdQuery, ApiResponse<UserSummaryDto>>
{
    public async Task<ApiResponse<UserSummaryDto>> Handle(
        GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userService.GetByIdAsync(request.UserId);
        if (user is null)
            return ApiResponse<UserSummaryDto>.FailureResult("User not found.");

        var roles = await userService.GetRolesAsync(user);

        return ApiResponse<UserSummaryDto>.SuccessResult(new UserSummaryDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            ChurchId = user.ChurchId,
            IsActive = user.IsActive,
            Roles = roles,
            CreatedAt = user.CreatedAt
        });
    }
}
