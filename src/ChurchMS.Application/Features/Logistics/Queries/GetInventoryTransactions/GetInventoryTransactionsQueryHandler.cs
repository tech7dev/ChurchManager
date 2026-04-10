using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Queries.GetInventoryTransactions;

public class GetInventoryTransactionsQueryHandler(
    IRepository<InventoryTransaction> transactionRepository,
    IRepository<InventoryItem> itemRepository,
    IRepository<ChurchEvent> eventRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetInventoryTransactionsQuery, ApiResponse<PagedResult<InventoryTransactionDto>>>
{
    public async Task<ApiResponse<PagedResult<InventoryTransactionDto>>> Handle(
        GetInventoryTransactionsQuery request, CancellationToken cancellationToken)
    {
        var item = await itemRepository.GetByIdAsync(request.ItemId, cancellationToken);

        var all = await transactionRepository.FindAsync(
            t => t.ItemId == request.ItemId,
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(t => t.TransactionDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<InventoryTransactionDto>();
        foreach (var t in paged)
        {
            string? eventTitle = null;
            if (t.RelatedEventId.HasValue)
            {
                var ev = await eventRepository.GetByIdAsync(t.RelatedEventId.Value, cancellationToken);
                eventTitle = ev?.Title;
            }

            string? recorderName = null;
            if (t.RecordedByMemberId.HasValue)
            {
                var recorder = await memberRepository.GetByIdAsync(t.RecordedByMemberId.Value, cancellationToken);
                recorderName = recorder is not null ? $"{recorder.FirstName} {recorder.LastName}" : null;
            }

            dtos.Add(new InventoryTransactionDto
            {
                Id = t.Id,
                ChurchId = t.ChurchId,
                ItemId = t.ItemId,
                ItemName = item?.Name ?? "",
                Type = t.Type,
                QuantityChange = t.QuantityChange,
                QuantityAfter = t.QuantityAfter,
                TransactionDate = t.TransactionDate,
                RelatedEventId = t.RelatedEventId,
                RelatedEventTitle = eventTitle,
                RecordedByMemberId = t.RecordedByMemberId,
                RecordedByName = recorderName,
                Notes = t.Notes,
                CreatedAt = t.CreatedAt
            });
        }

        return ApiResponse<PagedResult<InventoryTransactionDto>>.SuccessResult(new PagedResult<InventoryTransactionDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
