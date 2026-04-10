namespace ChurchMS.Application.Features.Evangelism.DTOs;

public class EvangelismTeamDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public Guid CampaignId { get; set; }
    public string Name { get; set; } = null!;
    public Guid? LeaderMemberId { get; set; }
    public string? LeaderName { get; set; }
    public string? Notes { get; set; }
    public int MemberCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
