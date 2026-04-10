using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Queries.GetCampaignList;

public record GetCampaignListQuery(
    EvangelismCampaignStatus? Status = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<EvangelismCampaignDto>>>;
