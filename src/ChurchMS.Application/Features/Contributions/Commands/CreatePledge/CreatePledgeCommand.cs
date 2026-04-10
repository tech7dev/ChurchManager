using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Commands.CreatePledge;

public record CreatePledgeCommand(
    Guid MemberId,
    Guid FundId,
    Guid? CampaignId,
    decimal PledgedAmount,
    string Currency,
    DateOnly StartDate,
    DateOnly EndDate,
    string? Notes) : IRequest<ApiResponse<PledgeDto>>;
