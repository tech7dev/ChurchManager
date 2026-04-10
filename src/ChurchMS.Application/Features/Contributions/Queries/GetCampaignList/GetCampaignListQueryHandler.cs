using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetCampaignList;

public class GetCampaignListQueryHandler(
    IRepository<ContributionCampaign> campaignRepository,
    IRepository<Contribution> contributionRepository)
    : IRequestHandler<GetCampaignListQuery, ApiResponse<PagedResult<CampaignDto>>>
{
    public async Task<ApiResponse<PagedResult<CampaignDto>>> Handle(
        GetCampaignListQuery request,
        CancellationToken cancellationToken)
    {
        var all = await campaignRepository.FindAsync(
            c => !request.Status.HasValue || c.Status == request.Status.Value,
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<CampaignDto>();
        foreach (var campaign in paged)
        {
            var contributions = await contributionRepository.FindAsync(
                c => c.CampaignId == campaign.Id, cancellationToken);

            var dto = campaign.Adapt<CampaignDto>();
            dto.RaisedAmount = contributions.Sum(c => c.Amount);
            dto.ContributionCount = contributions.Count;
            dtos.Add(dto);
        }

        var result = new PagedResult<CampaignDto> { Items = dtos, TotalCount = totalCount, Page = request.Page, PageSize = request.PageSize };
        return ApiResponse<PagedResult<CampaignDto>>.SuccessResult(result);
    }
}
