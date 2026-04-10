using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A piece of media content published by the church (sermon, music, ebook, etc.).
/// Can be free, paid, or members-only.
/// </summary>
public class MediaContent : TenantEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public MediaContentType ContentType { get; set; }
    public MediaContentStatus Status { get; set; } = MediaContentStatus.Draft;
    public MediaAccessType AccessType { get; set; } = MediaAccessType.Free;

    /// <summary>Price in the church's primary currency (null if free).</summary>
    public decimal? Price { get; set; }

    /// <summary>URL or blob path to the media file.</summary>
    public string? FileUrl { get; set; }

    /// <summary>URL or blob path to the thumbnail image.</summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>Duration in seconds (for video/audio).</summary>
    public int? DurationSeconds { get; set; }

    public long? FileSizeBytes { get; set; }

    public string? Tags { get; set; }

    public int DownloadCount { get; set; }
    public int ViewCount { get; set; }

    /// <summary>Member who created/authored this content.</summary>
    public Guid? AuthorMemberId { get; set; }

    public DateTime? PublishedAt { get; set; }

    // Navigation
    public Member? Author { get; set; }
    public ICollection<MediaPurchase> Purchases { get; set; } = [];
    public ICollection<MediaPromotion> Promotions { get; set; } = [];
}
