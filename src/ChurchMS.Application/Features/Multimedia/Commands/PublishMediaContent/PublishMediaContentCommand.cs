using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Commands.PublishMediaContent;

public record PublishMediaContentCommand(Guid ContentId) : IRequest<ApiResponse<MediaContentDto>>;
