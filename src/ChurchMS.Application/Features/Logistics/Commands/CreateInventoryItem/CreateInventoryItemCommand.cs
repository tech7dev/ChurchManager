using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Commands.CreateInventoryItem;

public record CreateInventoryItemCommand(
    string Name,
    string? Description,
    string? Category,
    int Quantity,
    string? Unit,
    int? MinQuantity,
    string? Location,
    string? SerialNumber,
    string? Notes
) : IRequest<ApiResponse<InventoryItemDto>>;
