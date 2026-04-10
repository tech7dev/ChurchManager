using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A sub-team within an evangelism campaign.
/// </summary>
public class EvangelismTeam : TenantEntity
{
    public Guid CampaignId { get; set; }
    public string Name { get; set; } = null!;
    public Guid? LeaderMemberId { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public EvangelismCampaign Campaign { get; set; } = null!;
    public Member? Leader { get; set; }
    public ICollection<EvangelismTeamMember> Members { get; set; } = [];
}
