using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetUserList;

public record GetUserListQuery(
    bool? IsActive = null,
    string? Role = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<UserSummaryDto>>>;
