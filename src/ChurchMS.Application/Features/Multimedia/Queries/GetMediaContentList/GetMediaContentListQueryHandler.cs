using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Queries.GetMediaContentList;

public class GetMediaContentListQueryHandler(
    IRepository<MediaContent> contentRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetMediaContentListQuery, ApiResponse<PagedResult<MediaContentDto>>>
{
    public async Task<ApiResponse<PagedResult<MediaContentDto>>> Handle(
        GetMediaContentListQuery request, CancellationToken cancellationToken)
    {
        var all = await contentRepository.FindAsync(
            c => (!request.ContentType.HasValue || c.ContentType == request.ContentType.Value)
              && (!request.Status.HasValue || c.Status == request.Status.Value)
              && (!request.AccessType.HasValue || c.AccessType == request.AccessType.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(c => c.PublishedAt ?? c.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<MediaContentDto>();
        foreach (var content in paged)
        {
            string? authorName = null;
            if (content.AuthorMemberId.HasValue)
            {
                var author = await memberRepository.GetByIdAsync(content.AuthorMemberId.Value, cancellationToken);
                authorName = author is not null ? $"{author.FirstName} {author.LastName}" : null;
            }

            dtos.Add(new MediaContentDto
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

        return ApiResponse<PagedResult<MediaContentDto>>.SuccessResult(new PagedResult<MediaContentDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
