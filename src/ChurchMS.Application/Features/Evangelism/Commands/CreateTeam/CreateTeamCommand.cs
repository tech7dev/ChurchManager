using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.CreateTeam;

public record CreateTeamCommand(
    Guid CampaignId,
    string Name,
    Guid? LeaderMemberId,
    string? Notes
) : IRequest<ApiResponse<EvangelismTeamDto>>;
