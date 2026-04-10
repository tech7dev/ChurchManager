using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Accounting.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Accounting.Commands.CreateBankAccount;

public class CreateBankAccountCommandHandler(
    IRepository<BankAccount> bankAccountRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateBankAccountCommand, ApiResponse<BankAccountDto>>
{
    public async Task<ApiResponse<BankAccountDto>> Handle(
        CreateBankAccountCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        // Enforce max 5 bank accounts per church
        var existingCount = await bankAccountRepository.CountAsync(
            b => b.ChurchId == churchId, cancellationToken);
        if (existingCount >= AppConstants.MaxBankAccountsPerChurch)
            throw new BadRequestException(
                $"Maximum of {AppConstants.MaxBankAccountsPerChurch} bank accounts allowed per church.");

        if (request.IsDefault)
        {
            var existing = await bankAccountRepository.FindAsync(
                b => b.ChurchId == churchId && b.IsDefault, cancellationToken);
            foreach (var b in existing)
            {
                b.IsDefault = false;
                bankAccountRepository.Update(b);
            }
        }

        var bankAccount = new BankAccount
        {
            ChurchId = churchId,
            AccountName = request.AccountName,
            AccountNumber = request.AccountNumber,
            BankName = request.BankName,
            BranchName = request.BranchName,
            SwiftCode = request.SwiftCode,
            RoutingNumber = request.RoutingNumber,
            AccountType = request.AccountType,
            Currency = request.Currency,
            CurrentBalance = 0,
            IsActive = true,
            IsDefault = request.IsDefault
        };

        await bankAccountRepository.AddAsync(bankAccount, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<BankAccountDto>.SuccessResult(
            bankAccount.Adapt<BankAccountDto>(), "Bank account created successfully.");
    }
}
