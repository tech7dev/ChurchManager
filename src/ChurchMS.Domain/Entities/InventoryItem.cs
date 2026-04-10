using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A physical item in the church's inventory (chairs, projectors, sound equipment, etc.).
/// </summary>
public class InventoryItem : TenantEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Category { get; set; }

    /// <summary>Current quantity in stock.</summary>
    public int Quantity { get; set; }

    /// <summary>Unit of measurement (pcs, kg, m, etc.).</summary>
    public string? Unit { get; set; }

    /// <summary>Alert when quantity falls below this threshold.</summary>
    public int? MinQuantity { get; set; }

    public string? Location { get; set; }
    public string? SerialNumber { get; set; }
    public InventoryItemStatus Status { get; set; } = InventoryItemStatus.Available;
    public string? Notes { get; set; }

    // Navigation
    public ICollection<InventoryTransaction> Transactions { get; set; } = [];
}
