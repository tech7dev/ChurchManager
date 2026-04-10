using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetPledgeList;

public record GetPledgeListQuery(
    Guid? MemberId = null,
    Guid? FundId = null,
    PledgeStatus? Status = null,
    int Page = 1,
    int PageSize = 10) : IRequest<ApiResponse<PagedResult<PledgeDto>>>;
