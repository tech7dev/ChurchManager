using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Multimedia.DTOs;

public class MediaContentDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public MediaContentType ContentType { get; set; }
    public MediaContentStatus Status { get; set; }
    public MediaAccessType AccessType { get; set; }
    public decimal? Price { get; set; }
    public string? FileUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int? DurationSeconds { get; set; }
    public long? FileSizeBytes { get; set; }
    public string? Tags { get; set; }
    public int DownloadCount { get; set; }
    public int ViewCount { get; set; }
    public Guid? AuthorMemberId { get; set; }
    public string? AuthorName { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
