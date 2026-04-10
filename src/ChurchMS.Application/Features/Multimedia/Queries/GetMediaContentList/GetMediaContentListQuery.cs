using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Queries.GetMediaContentList;

public record GetMediaContentListQuery(
    MediaContentType? ContentType = null,
    MediaContentStatus? Status = null,
    MediaAccessType? AccessType = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<MediaContentDto>>>;
