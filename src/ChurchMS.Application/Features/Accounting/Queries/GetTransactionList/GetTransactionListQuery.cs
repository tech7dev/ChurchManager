using ChurchMS.Application.Features.Accounting.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Accounting.Queries.GetTransactionList;

public record GetTransactionListQuery(
    Guid BankAccountId,
    TransactionType? Type = null,
    DateOnly? FromDate = null,
    DateOnly? ToDate = null,
    int Page = 1,
    int PageSize = 10) : IRequest<ApiResponse<PagedResult<AccountTransactionDto>>>;
