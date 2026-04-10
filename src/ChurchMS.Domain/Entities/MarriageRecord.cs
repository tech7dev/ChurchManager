using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A marriage event registered at the church.
/// One or both spouses may be members; the other may be a guest (name/phone only).
/// </summary>
public class MarriageRecord : TenantEntity
{
    /// <summary>First spouse — must be a church member.</summary>
    public Guid Spouse1MemberId { get; set; }

    /// <summary>Second spouse if also a member (null for external spouse).</summary>
    public Guid? Spouse2MemberId { get; set; }

    /// <summary>Name of second spouse when not a member.</summary>
    public string? Spouse2Name { get; set; }
    public string? Spouse2Phone { get; set; }

    public DateOnly MarriageDate { get; set; }

    /// <summary>Pastor or minister who officiated.</summary>
    public Guid? OfficiantMemberId { get; set; }

    public string? Location { get; set; }
    public string? Notes { get; set; }

    /// <summary>Certificate issued for this marriage (if any).</summary>
    public Guid? CertificateId { get; set; }

    // Navigation
    public Member Spouse1 { get; set; } = null!;
    public Member? Spouse2 { get; set; }
    public Member? Officiant { get; set; }
    public Certificate? Certificate { get; set; }
}
