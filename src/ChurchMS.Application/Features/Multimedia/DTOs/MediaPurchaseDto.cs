using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Multimedia.DTOs;

public class MediaPurchaseDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public Guid ContentId { get; set; }
    public string ContentTitle { get; set; } = null!;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = null!;
    public decimal Amount { get; set; }
    public MediaPurchaseStatus Status { get; set; }
    public string? PaymentReference { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public Guid? ActivatedByMemberId { get; set; }
    public string? ActivatedByName { get; set; }
    public DateTime CreatedAt { get; set; }
}
