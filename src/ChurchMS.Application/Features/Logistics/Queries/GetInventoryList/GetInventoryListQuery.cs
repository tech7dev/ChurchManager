using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Queries.GetInventoryList;

public record GetInventoryListQuery(
    string? Category = null,
    InventoryItemStatus? Status = null,
    bool LowStockOnly = false,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<InventoryItemDto>>>;
