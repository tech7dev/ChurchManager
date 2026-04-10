using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Queries.GetCampaignList;

public record GetCampaignListQuery(
    MessageChannel? Channel = null,
    MessageCampaignStatus? Status = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<MessageCampaignDto>>>;
