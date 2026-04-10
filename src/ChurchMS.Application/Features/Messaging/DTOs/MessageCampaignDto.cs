using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Messaging.DTOs;

public class MessageCampaignDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public MessageChannel Channel { get; set; }
    public MessageCampaignStatus Status { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? SentAt { get; set; }
    public Guid? SentByMemberId { get; set; }
    public string? SentByName { get; set; }
    public int RecipientCount { get; set; }
    public int DeliveredCount { get; set; }
    public int FailedCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
