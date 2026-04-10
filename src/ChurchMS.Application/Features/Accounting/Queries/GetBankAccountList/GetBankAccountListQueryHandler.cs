using ChurchMS.Application.Features.Accounting.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Accounting.Queries.GetBankAccountList;

public class GetBankAccountListQueryHandler(
    IRepository<BankAccount> bankAccountRepository)
    : IRequestHandler<GetBankAccountListQuery, ApiResponse<IList<BankAccountDto>>>
{
    public async Task<ApiResponse<IList<BankAccountDto>>> Handle(
        GetBankAccountListQuery request,
        CancellationToken cancellationToken)
    {
        var accounts = await bankAccountRepository.FindAsync(
            b => !request.ActiveOnly || b.IsActive,
            cancellationToken);

        return ApiResponse<IList<BankAccountDto>>.SuccessResult(accounts.Adapt<IList<BankAccountDto>>());
    }
}
