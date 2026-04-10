using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Churches.DTOs;

public class ChurchDto
{
    public Guid Id { get; set; }
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
    public string TimeZone { get; set; } = string.Empty;
    public string PrimaryCurrency { get; set; } = string.Empty;
    public string? SecondaryCurrency { get; set; }
    public ChurchStatus Status { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }
    public Guid? ParentChurchId { get; set; }
    public DateTime CreatedAt { get; set; }
}
