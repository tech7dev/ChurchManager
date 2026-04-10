using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Contributions.DTOs;

public class ContributionDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly ContributionDate { get; set; }
    public ContributionType Type { get; set; }
    public ContributionStatus Status { get; set; }
    public string? Notes { get; set; }
    public string? CheckNumber { get; set; }
    public string? TransactionReference { get; set; }
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public string? AnonymousDonorName { get; set; }
    public Guid FundId { get; set; }
    public string FundName { get; set; } = string.Empty;
    public Guid? CampaignId { get; set; }
    public string? CampaignName { get; set; }
    public bool IsRecurring { get; set; }
    public RecurrenceFrequency? RecurrenceFrequency { get; set; }
    public DateOnly? RecurrenceEndDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
