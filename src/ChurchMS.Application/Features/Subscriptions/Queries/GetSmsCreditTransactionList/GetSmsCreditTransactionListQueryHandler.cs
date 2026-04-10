using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Queries.GetSmsCreditTransactionList;

public class GetSmsCreditTransactionListQueryHandler(
    IRepository<SmsCreditTransaction> transactionRepository)
    : IRequestHandler<GetSmsCreditTransactionListQuery, ApiResponse<PagedResult<SmsCreditTransactionDto>>>
{
    public async Task<ApiResponse<PagedResult<SmsCreditTransactionDto>>> Handle(
        GetSmsCreditTransactionListQuery request, CancellationToken cancellationToken)
    {
        var all = await transactionRepository.FindAsync(
            t => !request.Type.HasValue || t.Type == request.Type.Value,
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(t => t.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new SmsCreditTransactionDto
            {
                Id = t.Id,
                ChurchId = t.ChurchId,
                Type = t.Type,
                Amount = t.Amount,
                BalanceBefore = t.BalanceBefore,
                BalanceAfter = t.BalanceAfter,
                Reference = t.Reference,
                Notes = t.Notes,
                CreatedAt = t.CreatedAt
            })
            .ToList();

        return ApiResponse<PagedResult<SmsCreditTransactionDto>>.SuccessResult(
            new PagedResult<SmsCreditTransactionDto>
            {
                Items = paged,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            });
    }
}
