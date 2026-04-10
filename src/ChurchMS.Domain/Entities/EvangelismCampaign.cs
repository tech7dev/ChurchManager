using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// An evangelism outreach campaign coordinated by the church.
/// </summary>
public class EvangelismCampaign : TenantEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public EvangelismCampaignStatus Status { get; set; } = EvangelismCampaignStatus.Draft;

    /// <summary>Target number of people to contact.</summary>
    public int? GoalContacts { get; set; }

    /// <summary>Member who leads this campaign.</summary>
    public Guid? LeaderMemberId { get; set; }

    public string? Notes { get; set; }

    // Navigation
    public Member? Leader { get; set; }
    public ICollection<EvangelismTeam> Teams { get; set; } = [];
    public ICollection<EvangelismContact> Contacts { get; set; } = [];
}
