using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A purchase (online or cash) of a paid media content item by a member.
/// Cash purchases require manual activation by staff.
/// </summary>
public class MediaPurchase : TenantEntity
{
    public Guid ContentId { get; set; }
    public Guid MemberId { get; set; }

    public decimal Amount { get; set; }
    public MediaPurchaseStatus Status { get; set; } = MediaPurchaseStatus.Pending;

    /// <summary>Payment gateway reference (Stripe, PayPal, Flutterwave, etc.).</summary>
    public string? PaymentReference { get; set; }

    /// <summary>Set when online payment is confirmed or cash is registered.</summary>
    public DateTime? PaidAt { get; set; }

    /// <summary>Set by staff when manually activating a cash purchase.</summary>
    public DateTime? ActivatedAt { get; set; }
    public Guid? ActivatedByMemberId { get; set; }

    // Navigation
    public MediaContent Content { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public Member? ActivatedBy { get; set; }
}
