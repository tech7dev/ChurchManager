using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A bulk messaging campaign sent via SMS, Email, or WhatsApp.
/// </summary>
public class MessageCampaign : TenantEntity
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public MessageChannel Channel { get; set; }
    public MessageCampaignStatus Status { get; set; } = MessageCampaignStatus.Draft;

    /// <summary>When the campaign is scheduled to be sent (null = send immediately).</summary>
    public DateTime? ScheduledAt { get; set; }
    public DateTime? SentAt { get; set; }

    /// <summary>Member who triggered the send.</summary>
    public Guid? SentByMemberId { get; set; }

    public int RecipientCount { get; set; }
    public int DeliveredCount { get; set; }
    public int FailedCount { get; set; }

    // Navigation
    public Member? SentBy { get; set; }
    public ICollection<MessageRecipient> Recipients { get; set; } = [];
}
