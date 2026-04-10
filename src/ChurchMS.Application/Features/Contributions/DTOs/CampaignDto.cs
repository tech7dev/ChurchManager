using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Contributions.DTOs;

public class CampaignDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal RaisedAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public CampaignStatus Status { get; set; }
    public Guid? FundId { get; set; }
    public string? FundName { get; set; }
    public int ContributionCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
