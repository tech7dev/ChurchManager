using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Commands.CreateInventoryItem;

public class CreateInventoryItemCommandHandler(
    IRepository<InventoryItem> itemRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateInventoryItemCommand, ApiResponse<InventoryItemDto>>
{
    public async Task<ApiResponse<InventoryItemDto>> Handle(
        CreateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<InventoryItemDto>.FailureResult("Church context required.");

        var item = new InventoryItem
        {
            ChurchId = churchId.Value,
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            Quantity = request.Quantity,
            Unit = request.Unit,
            MinQuantity = request.MinQuantity,
            Location = request.Location,
            SerialNumber = request.SerialNumber,
            Notes = request.Notes,
            Status = InventoryItemStatus.Available
        };

        await itemRepository.AddAsync(item, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<InventoryItemDto>.SuccessResult(MapToDto(item));
    }

    private static InventoryItemDto MapToDto(InventoryItem i) => new()
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
    };
}
