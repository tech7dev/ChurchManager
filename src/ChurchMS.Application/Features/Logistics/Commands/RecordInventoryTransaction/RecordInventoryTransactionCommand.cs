using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Commands.RecordInventoryTransaction;

public record RecordInventoryTransactionCommand(
    Guid ItemId,
    InventoryTransactionType Type,
    int QuantityChange,
    DateOnly TransactionDate,
    Guid? RelatedEventId,
    Guid? RecordedByMemberId,
    string? Notes
) : IRequest<ApiResponse<InventoryTransactionDto>>;
