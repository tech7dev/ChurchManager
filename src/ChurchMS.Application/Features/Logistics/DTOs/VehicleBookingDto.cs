using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Logistics.DTOs;

public class VehicleBookingDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public Guid VehicleId { get; set; }
    public string VehicleLabel { get; set; } = null!;
    public Guid? DriverMemberId { get; set; }
    public string? DriverName { get; set; }
    public Guid? RelatedEventId { get; set; }
    public string? RelatedEventTitle { get; set; }
    public string Purpose { get; set; } = null!;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public VehicleBookingStatus Status { get; set; }
    public Guid? ApprovedByMemberId { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
