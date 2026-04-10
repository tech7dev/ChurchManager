using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Queries.GetCampaignById;

public class GetCampaignByIdQueryHandler(
    IRepository<EvangelismCampaign> campaignRepository,
    IRepository<EvangelismTeam> teamRepository,
    IRepository<EvangelismContact> contactRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetCampaignByIdQuery, ApiResponse<EvangelismCampaignDto>>
{
    public async Task<ApiResponse<EvangelismCampaignDto>> Handle(
        GetCampaignByIdQuery request, CancellationToken cancellationToken)
    {
        var campaign = await campaignRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(EvangelismCampaign), request.Id);

        string? leaderName = null;
        if (campaign.LeaderMemberId.HasValue)
        {
            var leader = await memberRepository.GetByIdAsync(campaign.LeaderMemberId.Value, cancellationToken);
            leaderName = leader is not null ? $"{leader.FirstName} {leader.LastName}" : null;
        }

        var teams = await teamRepository.FindAsync(t => t.CampaignId == campaign.Id, cancellationToken);
        var contacts = await contactRepository.FindAsync(c => c.CampaignId == campaign.Id, cancellationToken);
        var convertedCount = contacts.Count(c => c.Status == Domain.Enums.ContactStatus.Converted);

        return ApiResponse<EvangelismCampaignDto>.SuccessResult(new EvangelismCampaignDto
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
}
