using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Evangelism.DTOs;

public class EvangelismCampaignDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public EvangelismCampaignStatus Status { get; set; }
    public int? GoalContacts { get; set; }
    public Guid? LeaderMemberId { get; set; }
    public string? LeaderName { get; set; }
    public string? Notes { get; set; }
    public int TeamCount { get; set; }
    public int ContactCount { get; set; }
    public int ConvertedCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
