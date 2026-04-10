using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Queries.GetContactList;

public record GetContactListQuery(
    Guid? CampaignId = null,
    Guid? TeamId = null,
    ContactStatus? Status = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<EvangelismContactDto>>>;
