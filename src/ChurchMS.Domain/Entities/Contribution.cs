using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Individual financial contribution (tithe, offering, donation, etc.).
/// </summary>
public class Contribution : TenantEntity
{
    public string ReferenceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly ContributionDate { get; set; }
    public ContributionType Type { get; set; }
    public ContributionStatus Status { get; set; } = ContributionStatus.Confirmed;
    public string? Notes { get; set; }
    public string? CheckNumber { get; set; }
    public string? TransactionReference { get; set; } // for mobile money / online

    // Who gave
    public Guid? MemberId { get; set; }
    public Member? Member { get; set; }
    public string? AnonymousDonorName { get; set; } // if walk-in, no member

    // Fund / Campaign
    public Guid FundId { get; set; }
    public Fund Fund { get; set; } = null!;
    public Guid? CampaignId { get; set; }
    public ContributionCampaign? Campaign { get; set; }

    // Recurring
    public bool IsRecurring { get; set; }
    public RecurrenceFrequency? RecurrenceFrequency { get; set; }
    public DateOnly? RecurrenceEndDate { get; set; }

    // Who recorded
    public Guid? RecordedByMemberId { get; set; }
}
