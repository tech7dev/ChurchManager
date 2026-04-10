using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Evangelism.DTOs;

public class EvangelismContactDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public Guid CampaignId { get; set; }
    public Guid? TeamId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}";
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public ContactStatus Status { get; set; }
    public Guid? AssignedToMemberId { get; set; }
    public string? AssignedToName { get; set; }
    public string? Notes { get; set; }
    public DateTime? ConvertedAt { get; set; }
    public Guid? ConvertedMemberId { get; set; }
    public int FollowUpCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
