using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Contributions.DTOs;

public class PledgeDto
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public Guid FundId { get; set; }
    public string FundName { get; set; } = string.Empty;
    public Guid? CampaignId { get; set; }
    public string? CampaignName { get; set; }
    public decimal PledgedAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount => PledgedAmount - PaidAmount;
    public string Currency { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public PledgeStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
