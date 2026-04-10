using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetUserList;

public class GetUserListQueryHandler(
    IUserService userService,
    ITenantService tenantService)
    : IRequestHandler<GetUserListQuery, ApiResponse<PagedResult<UserSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<UserSummaryDto>>> Handle(
        GetUserListQuery request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        var users = await userService.GetUsersAsync(
            tenantService.IsSuperAdmin() ? null : churchId,
            cancellationToken);

        // Build DTOs with roles
        var dtos = new List<UserSummaryDto>();
        foreach (var user in users)
        {
            var roles = await userService.GetRolesAsync(user);
            dtos.Add(new UserSummaryDto
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

        // Filter
        if (request.IsActive.HasValue)
            dtos = dtos.Where(u => u.IsActive == request.IsActive.Value).ToList();

        if (request.Role is not null)
            dtos = dtos.Where(u => u.Roles.Contains(request.Role)).ToList();

        // Paginate
        var totalCount = dtos.Count;
        var paged = dtos
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return ApiResponse<PagedResult<UserSummaryDto>>.SuccessResult(new PagedResult<UserSummaryDto>
        {
            Items = paged,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
