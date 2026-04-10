using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Commands.CreateMediaContent;

public record CreateMediaContentCommand(
    string Title,
    string? Description,
    MediaContentType ContentType,
    MediaAccessType AccessType,
    decimal? Price,
    string? FileUrl,
    string? ThumbnailUrl,
    int? DurationSeconds,
    long? FileSizeBytes,
    string? Tags,
    Guid? AuthorMemberId
) : IRequest<ApiResponse<MediaContentDto>>;
