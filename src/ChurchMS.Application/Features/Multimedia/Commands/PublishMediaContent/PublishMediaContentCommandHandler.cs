using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Commands.PublishMediaContent;

public class PublishMediaContentCommandHandler(
    IRepository<MediaContent> contentRepository,
    IRepository<Member> memberRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PublishMediaContentCommand, ApiResponse<MediaContentDto>>
{
    public async Task<ApiResponse<MediaContentDto>> Handle(
        PublishMediaContentCommand request, CancellationToken cancellationToken)
    {
        var content = await contentRepository.GetByIdAsync(request.ContentId, cancellationToken)
            ?? throw new NotFoundException(nameof(MediaContent), request.ContentId);

        if (content.Status == MediaContentStatus.Published)
            return ApiResponse<MediaContentDto>.FailureResult("Content is already published.");

        content.Status = MediaContentStatus.Published;
        content.PublishedAt ??= DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

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
