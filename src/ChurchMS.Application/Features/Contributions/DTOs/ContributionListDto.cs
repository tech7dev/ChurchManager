using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Contributions.DTOs;

public class ContributionListDto
{
    public Guid Id { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly ContributionDate { get; set; }
    public ContributionType Type { get; set; }
    public ContributionStatus Status { get; set; }
    public string? MemberName { get; set; }
    public string? AnonymousDonorName { get; set; }
    public string FundName { get; set; } = string.Empty;
    public string? CampaignName { get; set; }
}
