using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Queries.GetMediaContentById;

public class GetMediaContentByIdQueryHandler(
    IRepository<MediaContent> contentRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetMediaContentByIdQuery, ApiResponse<MediaContentDto>>
{
    public async Task<ApiResponse<MediaContentDto>> Handle(
        GetMediaContentByIdQuery request, CancellationToken cancellationToken)
    {
        var content = await contentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(MediaContent), request.Id);

        string? authorName = null;
        if (content.AuthorMemberId.HasValue)
        {
            var author = await memberRepository.GetByIdAsync(content.AuthorMemberId.Value, cancellationToken);
            authorName = author is not null ? $"{author.FirstName} {author.LastName}" : null;
        }

        return ApiResponse<MediaContentDto>.SuccessResult(new MediaContentDto
        {
            Id = content.Id,
            ChurchId = content.ChurchId,
            Title = content.Title,
            Description = content.Description,
            ContentType = content.ContentType,
            Status = content.Status,
            AccessType = content.AccessType,
            Price = content.Price,
            FileUrl = content.FileUrl,
            ThumbnailUrl = content.ThumbnailUrl,
            DurationSeconds = content.DurationSeconds,
            FileSizeBytes = content.FileSizeBytes,
            Tags = content.Tags,
            DownloadCount = content.DownloadCount,
            ViewCount = content.ViewCount,
            AuthorMemberId = content.AuthorMemberId,
            AuthorName = authorName,
            PublishedAt = content.PublishedAt,
            CreatedAt = content.CreatedAt
        });
    }
}
