using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A member's commitment to contribute a specified amount over time.
/// </summary>
public class Pledge : TenantEntity
{
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;
    public Guid FundId { get; set; }
    public Fund Fund { get; set; } = null!;
    public Guid? CampaignId { get; set; }
    public ContributionCampaign? Campaign { get; set; }
    public decimal PledgedAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public PledgeStatus Status { get; set; } = PledgeStatus.Active;
    public string? Notes { get; set; }
}
