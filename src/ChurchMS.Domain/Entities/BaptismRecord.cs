using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A baptism event for a member, linked to an optional certificate.
/// </summary>
public class BaptismRecord : TenantEntity
{
    public Guid MemberId { get; set; }
    public DateOnly BaptismDate { get; set; }

    /// <summary>Pastor or minister who officiated.</summary>
    public Guid? OfficiantMemberId { get; set; }

    public string? Location { get; set; }
    public string? Notes { get; set; }

    /// <summary>Certificate issued for this baptism (if any).</summary>
    public Guid? CertificateId { get; set; }

    // Navigation
    public Member Member { get; set; } = null!;
    public Member? Officiant { get; set; }
    public Certificate? Certificate { get; set; }
}
