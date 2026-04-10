using SQLite;

namespace ChurchMS.MAUI.Models;

[Table("Contributions")]
public class LocalContribution
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly ContributionDate { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? MemberName { get; set; }
    public string FundName { get; set; } = string.Empty;
    public string? CampaignName { get; set; }
    public bool IsSynced { get; set; }
    public DateTime CreatedAt { get; set; }
}
