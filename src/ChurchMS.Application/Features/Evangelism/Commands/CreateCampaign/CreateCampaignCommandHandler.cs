using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.CreateCampaign;

public class CreateCampaignCommandHandler(
    IRepository<EvangelismCampaign> campaignRepository,
    IRepository<Member> memberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCampaignCommand, ApiResponse<EvangelismCampaignDto>>
{
    public async Task<ApiResponse<EvangelismCampaignDto>> Handle(
        CreateCampaignCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<EvangelismCampaignDto>.FailureResult("Church context required.");

        var campaign = new EvangelismCampaign
        {
            ChurchId = churchId.Value,
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            GoalContacts = request.GoalContacts,
            LeaderMemberId = request.LeaderMemberId,
            Notes = request.Notes,
            Status = EvangelismCampaignStatus.Draft
        };

        await campaignRepository.AddAsync(campaign, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? leaderName = null;
        if (campaign.LeaderMemberId.HasValue)
        {
            var leader = await memberRepository.GetByIdAsync(campaign.LeaderMemberId.Value, cancellationToken);
            leaderName = leader is not null ? $"{leader.FirstName} {leader.LastName}" : null;
        }

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
            TeamCount = 0,
            ContactCount = 0,
            ConvertedCount = 0,
            CreatedAt = campaign.CreatedAt
        });
    }
}
