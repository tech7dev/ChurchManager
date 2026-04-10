using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Logistics.DTOs;

public class InventoryTransactionDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public InventoryTransactionType Type { get; set; }
    public int QuantityChange { get; set; }
    public int QuantityAfter { get; set; }
    public DateOnly TransactionDate { get; set; }
    public Guid? RelatedEventId { get; set; }
    public string? RelatedEventTitle { get; set; }
    public Guid? RecordedByMemberId { get; set; }
    public string? RecordedByName { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
