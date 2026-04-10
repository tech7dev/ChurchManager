using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A person contacted during an evangelism campaign.
/// May later become a member (tracked via ConvertedMemberId).
/// </summary>
public class EvangelismContact : TenantEntity
{
    public Guid CampaignId { get; set; }
    public Guid? TeamId { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    public ContactStatus Status { get; set; } = ContactStatus.New;

    /// <summary>Member responsible for following up with this contact.</summary>
    public Guid? AssignedToMemberId { get; set; }

    public string? Notes { get; set; }

    /// <summary>Date the contact converted (accepted faith / joined church).</summary>
    public DateTime? ConvertedAt { get; set; }

    /// <summary>Member record created when this contact converted.</summary>
    public Guid? ConvertedMemberId { get; set; }

    // Navigation
    public EvangelismCampaign Campaign { get; set; } = null!;
    public EvangelismTeam? Team { get; set; }
    public Member? AssignedTo { get; set; }
    public Member? ConvertedMember { get; set; }
    public ICollection<EvangelismFollowUp> FollowUps { get; set; } = [];
}
