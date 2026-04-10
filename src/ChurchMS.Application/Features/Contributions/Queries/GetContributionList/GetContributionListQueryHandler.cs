using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetContributionList;

public class GetContributionListQueryHandler(
    IRepository<Contribution> contributionRepository,
    ITenantService tenantService)
    : IRequestHandler<GetContributionListQuery, ApiResponse<PagedResult<ContributionListDto>>>
{
    public async Task<ApiResponse<PagedResult<ContributionListDto>>> Handle(
        GetContributionListQuery request,
        CancellationToken cancellationToken)
    {
        var all = await contributionRepository.FindAsync(c =>
            (!request.FundId.HasValue || c.FundId == request.FundId.Value) &&
            (!request.CampaignId.HasValue || c.CampaignId == request.CampaignId.Value) &&
            (!request.MemberId.HasValue || c.MemberId == request.MemberId.Value) &&
            (!request.Type.HasValue || c.Type == request.Type.Value) &&
            (!request.Status.HasValue || c.Status == request.Status.Value) &&
            (!request.FromDate.HasValue || c.ContributionDate >= request.FromDate.Value) &&
            (!request.ToDate.HasValue || c.ContributionDate <= request.ToDate.Value),
            cancellationToken);

        var totalCount = all.Count;
        var items = all
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Adapt<List<ContributionListDto>>();

        var result = new PagedResult<ContributionListDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
        return ApiResponse<PagedResult<ContributionListDto>>.SuccessResult(result);
    }
}
