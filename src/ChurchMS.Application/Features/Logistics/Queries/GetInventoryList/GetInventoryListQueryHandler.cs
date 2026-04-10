using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Queries.GetInventoryList;

public class GetInventoryListQueryHandler(
    IRepository<InventoryItem> itemRepository)
    : IRequestHandler<GetInventoryListQuery, ApiResponse<PagedResult<InventoryItemDto>>>
{
    public async Task<ApiResponse<PagedResult<InventoryItemDto>>> Handle(
        GetInventoryListQuery request, CancellationToken cancellationToken)
    {
        var all = await itemRepository.FindAsync(
            i => (request.Category == null || i.Category == request.Category)
              && (!request.Status.HasValue || i.Status == request.Status.Value)
              && (!request.LowStockOnly || (i.MinQuantity.HasValue && i.Quantity <= i.MinQuantity.Value)),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderBy(i => i.Category)
            .ThenBy(i => i.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = paged.Select(i => new InventoryItemDto
        {
            Id = i.Id,
            ChurchId = i.ChurchId,
            Name = i.Name,
            Description = i.Description,
            Category = i.Category,
            Quantity = i.Quantity,
            Unit = i.Unit,
            MinQuantity = i.MinQuantity,
            Location = i.Location,
            SerialNumber = i.SerialNumber,
            Status = i.Status,
            Notes = i.Notes,
            CreatedAt = i.CreatedAt
        }).ToList();

        return ApiResponse<PagedResult<InventoryItemDto>>.SuccessResult(new PagedResult<InventoryItemDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
