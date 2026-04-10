using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// An issued certificate (baptism, marriage, membership, etc.) for a member.
/// </summary>
public class Certificate : TenantEntity
{
    public CertificateType Type { get; set; }
    public string CertificateNumber { get; set; } = null!;

    /// <summary>Primary recipient member.</summary>
    public Guid MemberId { get; set; }

    public DateOnly IssuedDate { get; set; }
    public Guid? IssuedByMemberId { get; set; }

    /// <summary>URL or blob path of the generated certificate file.</summary>
    public string? FileUrl { get; set; }

    public string? Notes { get; set; }

    // Navigation
    public Member Member { get; set; } = null!;
    public Member? IssuedBy { get; set; }
}
