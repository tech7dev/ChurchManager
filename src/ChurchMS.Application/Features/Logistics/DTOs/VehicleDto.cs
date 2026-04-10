using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Logistics.DTOs;

public class VehicleDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int? Year { get; set; }
    public string? PlateNumber { get; set; }
    public int? Capacity { get; set; }
    public VehicleStatus Status { get; set; }
    public string? Color { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
