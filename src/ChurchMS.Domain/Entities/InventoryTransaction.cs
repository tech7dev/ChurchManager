using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A stock movement (check-in, check-out, adjustment) for an inventory item.
/// </summary>
public class InventoryTransaction : TenantEntity
{
    public Guid ItemId { get; set; }
    public InventoryTransactionType Type { get; set; }

    /// <summary>Positive = added to stock, negative = removed from stock.</summary>
    public int QuantityChange { get; set; }

    /// <summary>Quantity on hand after this transaction.</summary>
    public int QuantityAfter { get; set; }

    public DateOnly TransactionDate { get; set; }
    public Guid? RelatedEventId { get; set; }
    public Guid? RecordedByMemberId { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public InventoryItem Item { get; set; } = null!;
    public ChurchEvent? RelatedEvent { get; set; }
    public Member? RecordedBy { get; set; }
}
