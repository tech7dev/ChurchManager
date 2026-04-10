using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.CreateTeam;

public class CreateTeamCommandHandler(
    IRepository<EvangelismCampaign> campaignRepository,
    IRepository<EvangelismTeam> teamRepository,
    IRepository<Member> memberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTeamCommand, ApiResponse<EvangelismTeamDto>>
{
    public async Task<ApiResponse<EvangelismTeamDto>> Handle(
        CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<EvangelismTeamDto>.FailureResult("Church context required.");

        var campaign = await campaignRepository.GetByIdAsync(request.CampaignId, cancellationToken)
            ?? throw new NotFoundException(nameof(EvangelismCampaign), request.CampaignId);

        var team = new EvangelismTeam
        {
            ChurchId = churchId.Value,
            CampaignId = campaign.Id,
            Name = request.Name,
            LeaderMemberId = request.LeaderMemberId,
            Notes = request.Notes
        };

        await teamRepository.AddAsync(team, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? leaderName = null;
        if (team.LeaderMemberId.HasValue)
        {
            var leader = await memberRepository.GetByIdAsync(team.LeaderMemberId.Value, cancellationToken);
            leaderName = leader is not null ? $"{leader.FirstName} {leader.LastName}" : null;
        }

        return ApiResponse<EvangelismTeamDto>.SuccessResult(new EvangelismTeamDto
        {
            Id = team.Id,
            ChurchId = team.ChurchId,
            CampaignId = team.CampaignId,
            Name = team.Name,
            LeaderMemberId = team.LeaderMemberId,
            LeaderName = leaderName,
            Notes = team.Notes,
            MemberCount = 0,
            CreatedAt = team.CreatedAt
        });
    }
}
