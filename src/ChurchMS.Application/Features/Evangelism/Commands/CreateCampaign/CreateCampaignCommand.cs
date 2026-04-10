using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.CreateCampaign;

public record CreateCampaignCommand(
    string Name,
    string? Description,
    DateOnly StartDate,
    DateOnly? EndDate,
    int? GoalContacts,
    Guid? LeaderMemberId,
    string? Notes
) : IRequest<ApiResponse<EvangelismCampaignDto>>;
