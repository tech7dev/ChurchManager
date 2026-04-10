using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A booking/reservation of a church vehicle for a specific purpose or event.
/// </summary>
public class VehicleBooking : TenantEntity
{
    public Guid VehicleId { get; set; }
    public Guid? DriverMemberId { get; set; }
    public Guid? RelatedEventId { get; set; }

    public string Purpose { get; set; } = null!;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public VehicleBookingStatus Status { get; set; } = VehicleBookingStatus.Pending;

    public Guid? ApprovedByMemberId { get; set; }
    public DateTime? ApprovedAt { get; set; }

    public string? Notes { get; set; }

    // Navigation
    public Vehicle Vehicle { get; set; } = null!;
    public Member? Driver { get; set; }
    public ChurchEvent? RelatedEvent { get; set; }
    public Member? ApprovedBy { get; set; }
}
