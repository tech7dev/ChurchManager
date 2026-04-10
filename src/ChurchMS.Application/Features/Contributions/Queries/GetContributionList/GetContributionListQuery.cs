using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetContributionList;

public record GetContributionListQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    Guid? FundId = null,
    Guid? CampaignId = null,
    Guid? MemberId = null,
    ContributionType? Type = null,
    ContributionStatus? Status = null,
    DateOnly? FromDate = null,
    DateOnly? ToDate = null) : IRequest<ApiResponse<PagedResult<ContributionListDto>>>;
