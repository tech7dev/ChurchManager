using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A vehicle owned or managed by the church.
/// </summary>
public class Vehicle : TenantEntity
{
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int? Year { get; set; }
    public string? PlateNumber { get; set; }
    public int? Capacity { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.Available;
    public string? Color { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public ICollection<VehicleBooking> Bookings { get; set; } = [];
}
