using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetPledgeList;

public class GetPledgeListQueryHandler(
    IRepository<Pledge> pledgeRepository)
    : IRequestHandler<GetPledgeListQuery, ApiResponse<PagedResult<PledgeDto>>>
{
    public async Task<ApiResponse<PagedResult<PledgeDto>>> Handle(
        GetPledgeListQuery request,
        CancellationToken cancellationToken)
    {
        var all = await pledgeRepository.FindAsync(p =>
            (!request.MemberId.HasValue || p.MemberId == request.MemberId.Value) &&
            (!request.FundId.HasValue || p.FundId == request.FundId.Value) &&
            (!request.Status.HasValue || p.Status == request.Status.Value),
            cancellationToken);

        var totalCount = all.Count;
        var items = all
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Adapt<List<PledgeDto>>();

        var result = new PagedResult<PledgeDto> { Items = items, TotalCount = totalCount, Page = request.Page, PageSize = request.PageSize };
        return ApiResponse<PagedResult<PledgeDto>>.SuccessResult(result);
    }
}
