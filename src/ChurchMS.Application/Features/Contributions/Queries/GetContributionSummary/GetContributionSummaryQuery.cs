using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetContributionSummary;

public record GetContributionSummaryQuery(
    int? Year = null,
    int? Month = null,
    Guid? FundId = null) : IRequest<ApiResponse<ContributionSummaryDto>>;
