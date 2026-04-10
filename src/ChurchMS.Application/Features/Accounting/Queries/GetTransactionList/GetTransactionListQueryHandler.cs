using ChurchMS.Application.Features.Accounting.DTOs;
using ChurchMS.Application.Exceptions;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Accounting.Queries.GetTransactionList;

public class GetTransactionListQueryHandler(
    IRepository<AccountTransaction> transactionRepository,
    IRepository<BankAccount> bankAccountRepository)
    : IRequestHandler<GetTransactionListQuery, ApiResponse<PagedResult<AccountTransactionDto>>>
{
    public async Task<ApiResponse<PagedResult<AccountTransactionDto>>> Handle(
        GetTransactionListQuery request,
        CancellationToken cancellationToken)
    {
        var bankAccount = await bankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken)
            ?? throw new NotFoundException(nameof(BankAccount), request.BankAccountId);

        var all = await transactionRepository.FindAsync(t =>
            t.BankAccountId == request.BankAccountId &&
            (!request.Type.HasValue || t.Type == request.Type.Value) &&
            (!request.FromDate.HasValue || t.TransactionDate >= request.FromDate.Value) &&
            (!request.ToDate.HasValue || t.TransactionDate <= request.ToDate.Value),
            cancellationToken);

        var totalCount = all.Count;
        var items = all
            .OrderByDescending(t => t.TransactionDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = items.Adapt<List<AccountTransactionDto>>();
        foreach (var dto in dtos)
            dto.BankAccountName = bankAccount.AccountName;

        var result = new PagedResult<AccountTransactionDto> { Items = dtos, TotalCount = totalCount, Page = request.Page, PageSize = request.PageSize };
        return ApiResponse<PagedResult<AccountTransactionDto>>.SuccessResult(result);
    }
}
