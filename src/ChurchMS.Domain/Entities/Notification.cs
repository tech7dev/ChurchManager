using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// In-app notification delivered to a specific user.
/// </summary>
public class Notification : TenantEntity
{
    /// <summary>The AppUser who receives this notification.</summary>
    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public NotificationType Type { get; set; } = NotificationType.Info;

    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }

    /// <summary>E.g. "Event", "Contribution", "Appointment".</summary>
    public string? RelatedEntityType { get; set; }
    public Guid? RelatedEntityId { get; set; }

    /// <summary>Optional deep-link URL for the admin or mobile app.</summary>
    public string? ActionUrl { get; set; }
}
