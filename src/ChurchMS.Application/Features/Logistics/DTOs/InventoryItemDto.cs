using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Logistics.DTOs;

public class InventoryItemDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public int Quantity { get; set; }
    public string? Unit { get; set; }
    public int? MinQuantity { get; set; }
    public bool IsLowStock => MinQuantity.HasValue && Quantity <= MinQuantity.Value;
    public string? Location { get; set; }
    public string? SerialNumber { get; set; }
    public InventoryItemStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
