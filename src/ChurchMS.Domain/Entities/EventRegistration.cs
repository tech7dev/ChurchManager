using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Member or guest registration for a church event.
/// </summary>
public class EventRegistration : TenantEntity
{
    public Guid EventId { get; set; }
    public ChurchEvent Event { get; set; } = null!;

    // Who registered (member or walk-in)
    public Guid? MemberId { get; set; }
    public Member? Member { get; set; }
    public string? GuestName { get; set; }
    public string? GuestEmail { get; set; }
    public string? GuestPhone { get; set; }

    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;
    public string? RegistrationCode { get; set; } // QR code for check-in

    // Payment (if event has a fee)
    public decimal? AmountPaid { get; set; }
    public bool IsPaid { get; set; }

    public DateTime? CheckedInAt { get; set; }
    public string? Notes { get; set; }
}
