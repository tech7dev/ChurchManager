using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Queries.GetInventoryTransactions;

public record GetInventoryTransactionsQuery(
    Guid ItemId,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<InventoryTransactionDto>>>;
