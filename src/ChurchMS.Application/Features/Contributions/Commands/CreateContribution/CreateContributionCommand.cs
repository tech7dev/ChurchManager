using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Commands.CreateContribution;

public record CreateContributionCommand(
    decimal Amount,
    string Currency,
    DateOnly ContributionDate,
    ContributionType Type,
    string? Notes,
    string? CheckNumber,
    string? TransactionReference,
    Guid? MemberId,
    string? AnonymousDonorName,
    Guid FundId,
    Guid? CampaignId,
    bool IsRecurring,
    RecurrenceFrequency? RecurrenceFrequency,
    DateOnly? RecurrenceEndDate) : IRequest<ApiResponse<ContributionDto>>;
