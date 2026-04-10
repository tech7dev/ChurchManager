using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Fundraising campaign with a target and date range.
/// </summary>
public class ContributionCampaign : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal TargetAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public CampaignStatus Status { get; set; } = CampaignStatus.Draft;
    public Guid? FundId { get; set; }
    public Fund? Fund { get; set; }
    public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
}
