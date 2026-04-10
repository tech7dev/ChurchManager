using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetContributionReport;

public record GetContributionReportQuery(
    DateTime From,
    DateTime To,
    Guid? FundId = null,
    Guid? CampaignId = null,
    Guid? MemberId = null,
    int Page = 1,
    int PageSize = 50
) : IRequest<ApiResponse<ContributionReportDto>>;
