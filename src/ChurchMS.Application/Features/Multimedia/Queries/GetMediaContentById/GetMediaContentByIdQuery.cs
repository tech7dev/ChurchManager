using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Queries.GetMediaContentById;

public record GetMediaContentByIdQuery(Guid Id) : IRequest<ApiResponse<MediaContentDto>>;
