using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A file document stored for a church (contracts, reports, correspondence, etc.).
/// </summary>
public class Document : TenantEntity
{
    public string Title { get; set; } = null!;
    public string FileName { get; set; } = null!;

    /// <summary>URL or blob path of the stored file.</summary>
    public string FileUrl { get; set; } = null!;

    /// <summary>File size in bytes.</summary>
    public long FileSize { get; set; }

    public string? ContentType { get; set; }
    public DocumentType Type { get; set; } = DocumentType.General;

    /// <summary>Optional member this document is linked to (e.g. membership form).</summary>
    public Guid? MemberId { get; set; }

    public Guid? UploadedByMemberId { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public Member? Member { get; set; }
    public Member? UploadedBy { get; set; }
}
