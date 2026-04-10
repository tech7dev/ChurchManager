using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A single recipient in a message campaign (member or guest).
/// </summary>
public class MessageRecipient : TenantEntity
{
    public Guid CampaignId { get; set; }

    /// <summary>Linked member (null for guest recipients).</summary>
    public Guid? MemberId { get; set; }

    /// <summary>Guest contact details when MemberId is null.</summary>
    public string? GuestName { get; set; }
    public string? GuestPhone { get; set; }
    public string? GuestEmail { get; set; }

    public MessageRecipientStatus Status { get; set; } = MessageRecipientStatus.Pending;
    public DateTime? SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string? ErrorMessage { get; set; }

    // Navigation
    public MessageCampaign Campaign { get; set; } = null!;
    public Member? Member { get; set; }
}
