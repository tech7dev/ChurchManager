using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Queries.GetMediaPurchaseList;

public record GetMediaPurchaseListQuery(
    Guid? ContentId = null,
    Guid? MemberId = null,
    MediaPurchaseStatus? Status = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<MediaPurchaseDto>>>;
