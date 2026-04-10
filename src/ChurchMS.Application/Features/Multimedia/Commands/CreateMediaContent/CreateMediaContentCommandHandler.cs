using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Commands.CreateMediaContent;

public class CreateMediaContentCommandHandler(
    IRepository<MediaContent> contentRepository,
    IRepository<Member> memberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMediaContentCommand, ApiResponse<MediaContentDto>>
{
    public async Task<ApiResponse<MediaContentDto>> Handle(
        CreateMediaContentCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<MediaContentDto>.FailureResult("Church context required.");

        if (request.AccessType == MediaAccessType.Paid && (!request.Price.HasValue || request.Price <= 0))
            return ApiResponse<MediaContentDto>.FailureResult("Paid content must have a price greater than zero.");

        var content = new MediaContent
        {
            ChurchId = churchId.Value,
            Title = request.Title,
            Description = request.Description,
            ContentType = request.ContentType,
            AccessType = request.AccessType,
            Price = request.AccessType == MediaAccessType.Paid ? request.Price : null,
            FileUrl = request.FileUrl,
            ThumbnailUrl = request.ThumbnailUrl,
            DurationSeconds = request.DurationSeconds,
            FileSizeBytes = request.FileSizeBytes,
            Tags = request.Tags,
            AuthorMemberId = request.AuthorMemberId,
            Status = MediaContentStatus.Draft
        };

        await contentRepository.AddAsync(content, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? authorName = null;
        if (content.AuthorMemberId.HasValue)
        {
            var author = await memberRepository.GetByIdAsync(content.AuthorMemberId.Value, cancellationToken);
            authorName = author is not null ? $"{author.FirstName} {author.LastName}" : null;
        }

        return ApiResponse<MediaContentDto>.SuccessResult(MapToDto(content, authorName));
    }

    private static MediaContentDto MapToDto(MediaContent c, string? authorName) => new()
    {
        Id = c.Id,
        ChurchId = c.ChurchId,
        Title = c.Title,
        Description = c.Description,
        ContentType = c.ContentType,
        Status = c.Status,
        AccessType = c.AccessType,
        Price = c.Price,
        FileUrl = c.FileUrl,
        ThumbnailUrl = c.ThumbnailUrl,
        DurationSeconds = c.DurationSeconds,
        FileSizeBytes = c.FileSizeBytes,
        Tags = c.Tags,
        DownloadCount = c.DownloadCount,
        ViewCount = c.ViewCount,
        AuthorMemberId = c.AuthorMemberId,
        AuthorName = authorName,
        PublishedAt = c.PublishedAt,
        CreatedAt = c.CreatedAt
    };
}
