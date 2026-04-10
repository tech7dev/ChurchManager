using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Commands.RecordInventoryTransaction;

public class RecordInventoryTransactionCommandHandler(
    IRepository<InventoryItem> itemRepository,
    IRepository<InventoryTransaction> transactionRepository,
    IRepository<ChurchEvent> eventRepository,
    IRepository<Member> memberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RecordInventoryTransactionCommand, ApiResponse<InventoryTransactionDto>>
{
    public async Task<ApiResponse<InventoryTransactionDto>> Handle(
        RecordInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<InventoryTransactionDto>.FailureResult("Church context required.");

        var item = await itemRepository.GetByIdAsync(request.ItemId, cancellationToken)
            ?? throw new NotFoundException(nameof(InventoryItem), request.ItemId);

        var newQuantity = item.Quantity + request.QuantityChange;
        if (newQuantity < 0)
            return ApiResponse<InventoryTransactionDto>.FailureResult(
                $"Insufficient stock. Current quantity: {item.Quantity}, change: {request.QuantityChange}.");

        item.Quantity = newQuantity;

        var transaction = new InventoryTransaction
        {
            ChurchId = churchId.Value,
            ItemId = item.Id,
            Type = request.Type,
            QuantityChange = request.QuantityChange,
            QuantityAfter = newQuantity,
            TransactionDate = request.TransactionDate,
            RelatedEventId = request.RelatedEventId,
            RecordedByMemberId = request.RecordedByMemberId,
            Notes = request.Notes
        };

        await transactionRepository.AddAsync(transaction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? eventTitle = null;
        if (transaction.RelatedEventId.HasValue)
        {
            var ev = await eventRepository.GetByIdAsync(transaction.RelatedEventId.Value, cancellationToken);
            eventTitle = ev?.Title;
        }

        string? recordedByName = null;
        if (transaction.RecordedByMemberId.HasValue)
        {
            var recorder = await memberRepository.GetByIdAsync(transaction.RecordedByMemberId.Value, cancellationToken);
            recordedByName = recorder is not null ? $"{recorder.FirstName} {recorder.LastName}" : null;
        }

        return ApiResponse<InventoryTransactionDto>.SuccessResult(new InventoryTransactionDto
        {
            Id = transaction.Id,
            ChurchId = transaction.ChurchId,
            ItemId = transaction.ItemId,
            ItemName = item.Name,
            Type = transaction.Type,
            QuantityChange = transaction.QuantityChange,
            QuantityAfter = transaction.QuantityAfter,
            TransactionDate = transaction.TransactionDate,
            RelatedEventId = transaction.RelatedEventId,
            RelatedEventTitle = eventTitle,
            RecordedByMemberId = transaction.RecordedByMemberId,
            RecordedByName = recordedByName,
            Notes = transaction.Notes,
            CreatedAt = transaction.CreatedAt
        });
    }
}
