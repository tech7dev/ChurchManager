using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Queries.GetCampaignList;

public class GetCampaignListQueryHandler(
    IRepository<EvangelismCampaign> campaignRepository,
    IRepository<EvangelismTeam> teamRepository,
    IRepository<EvangelismContact> contactRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetCampaignListQuery, ApiResponse<PagedResult<EvangelismCampaignDto>>>
{
    public async Task<ApiResponse<PagedResult<EvangelismCampaignDto>>> Handle(
        GetCampaignListQuery request, CancellationToken cancellationToken)
    {
        var all = await campaignRepository.FindAsync(
            c => !request.Status.HasValue || c.Status == request.Status.Value,
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(c => c.StartDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<EvangelismCampaignDto>();
        foreach (var campaign in paged)
        {
            string? leaderName = null;
            if (campaign.LeaderMemberId.HasValue)
            {
                var leader = await memberRepository.GetByIdAsync(campaign.LeaderMemberId.Value, cancellationToken);
                leaderName = leader is not null ? $"{leader.FirstName} {leader.LastName}" : null;
            }

            var teams = await teamRepository.FindAsync(t => t.CampaignId == campaign.Id, cancellationToken);
            var contacts = await contactRepository.FindAsync(c => c.CampaignId == campaign.Id, cancellationToken);
            var convertedCount = contacts.Count(c => c.Status == Domain.Enums.ContactStatus.Converted);

            dtos.Add(new EvangelismCampaignDto
            {
                Id = campaign.Id,
                ChurchId = campaign.ChurchId,
                Name = campaign.Name,
                Description = campaign.Description,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                Status = campaign.Status,
                GoalContacts = campaign.GoalContacts,
                LeaderMemberId = campaign.LeaderMemberId,
                LeaderName = leaderName,
                Notes = campaign.Notes,
                TeamCount = teams.Count,
                ContactCount = contacts.Count,
                ConvertedCount = convertedCount,
                CreatedAt = campaign.CreatedAt
            });
        }

        return ApiResponse<PagedResult<EvangelismCampaignDto>>.SuccessResult(new PagedResult<EvangelismCampaignDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
