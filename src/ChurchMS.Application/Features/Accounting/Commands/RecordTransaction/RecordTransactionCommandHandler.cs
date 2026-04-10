using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Accounting.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Accounting.Commands.RecordTransaction;

public class RecordTransactionCommandHandler(
    IRepository<AccountTransaction> transactionRepository,
    IRepository<BankAccount> bankAccountRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<RecordTransactionCommand, ApiResponse<AccountTransactionDto>>
{
    public async Task<ApiResponse<AccountTransactionDto>> Handle(
        RecordTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var bankAccount = await bankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken)
            ?? throw new NotFoundException(nameof(BankAccount), request.BankAccountId);

        // Update balance
        if (request.Type == TransactionType.Credit)
            bankAccount.CurrentBalance += request.Amount;
        else
            bankAccount.CurrentBalance -= request.Amount;

        bankAccountRepository.Update(bankAccount);

        var transaction = new AccountTransaction
        {
            ChurchId = churchId,
            BankAccountId = request.BankAccountId,
            Type = request.Type,
            Amount = request.Amount,
            Currency = request.Currency,
            TransactionDate = request.TransactionDate,
            Description = request.Description ?? string.Empty,
            Reference = request.Reference,
            RunningBalance = bankAccount.CurrentBalance
        };

        await transactionRepository.AddAsync(transaction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = transaction.Adapt<AccountTransactionDto>();
        dto.BankAccountName = bankAccount.AccountName;

        return ApiResponse<AccountTransactionDto>.SuccessResult(dto, "Transaction recorded successfully.");
    }
}
