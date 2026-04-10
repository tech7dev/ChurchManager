using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Core tenant entity representing a church organization.
/// A church IS the tenant — it does not inherit TenantEntity.
/// </summary>
public class Church : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string Country { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string TimeZone { get; set; } = "UTC";
    public string PrimaryCurrency { get; set; } = "USD";
    public string? SecondaryCurrency { get; set; }
    public ChurchStatus Status { get; set; } = ChurchStatus.Active;
    public SubscriptionPlan SubscriptionPlan { get; set; } = SubscriptionPlan.Free;

    // Self-referencing hierarchy
    public Guid? ParentChurchId { get; set; }
    public Church? ParentChurch { get; set; }
    public ICollection<Church> ChildChurches { get; set; } = new List<Church>();
}
