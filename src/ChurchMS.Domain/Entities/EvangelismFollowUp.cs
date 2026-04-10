using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A follow-up interaction with an evangelism contact.
/// </summary>
public class EvangelismFollowUp : TenantEntity
{
    public Guid ContactId { get; set; }
    public FollowUpMethod Method { get; set; }
    public DateOnly FollowUpDate { get; set; }
    public string? Notes { get; set; }

    /// <summary>Member who conducted this follow-up.</summary>
    public Guid? ConductedByMemberId { get; set; }

    // Navigation
    public EvangelismContact Contact { get; set; } = null!;
    public Member? ConductedBy { get; set; }
}
