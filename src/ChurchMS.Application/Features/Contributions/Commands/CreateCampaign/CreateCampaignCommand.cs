using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Commands.CreateCampaign;

public record CreateCampaignCommand(
    string Name,
    string? Description,
    decimal TargetAmount,
    string Currency,
    DateOnly StartDate,
    DateOnly EndDate,
    Guid? FundId) : IRequest<ApiResponse<CampaignDto>>;
