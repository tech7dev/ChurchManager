using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Queries.GetFollowUpList;

public record GetFollowUpListQuery(
    Guid ContactId,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<EvangelismFollowUpDto>>>;
