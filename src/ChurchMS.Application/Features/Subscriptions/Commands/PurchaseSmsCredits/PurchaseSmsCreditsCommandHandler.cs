using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.PurchaseSmsCredits;

public class PurchaseSmsCreditsCommandHandler(
    IRepository<SmsCredit> smsCreditRepository,
    IRepository<SmsCreditTransaction> transactionRepository,
    IRepository<Invoice> invoiceRepository,
    ITenantService tenantService,
    IDateTimeService dateTimeService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PurchaseSmsCreditsCommand, ApiResponse<SmsCreditDto>>
{
    public async Task<ApiResponse<SmsCreditDto>> Handle(
        PurchaseSmsCreditsCommand request, CancellationToken cancellationToken)
    {
        if (request.Credits <= 0)
            return ApiResponse<SmsCreditDto>.FailureResult("Credits must be a positive number.");

        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<SmsCreditDto>.FailureResult("Church context required.");

        // Get or create the SMS credit wallet
        var wallets = await smsCreditRepository.FindAsync(
            w => w.ChurchId == churchId.Value, cancellationToken);

        SmsCredit wallet;
        if (wallets.Count == 0)
        {
            wallet = new SmsCredit { ChurchId = churchId.Value };
            await smsCreditRepository.AddAsync(wallet, cancellationToken);
        }
        else
        {
            wallet = wallets[0];
        }

        var balanceBefore = wallet.Balance;
        wallet.Balance += request.Credits;
        wallet.TotalPurchased += request.Credits;

        var transaction = new SmsCreditTransaction
        {
            ChurchId = churchId.Value,
            SmsCreditId = wallet.Id,
            Type = SmsCreditTransactionType.Purchase,
            Amount = request.Credits,
            BalanceBefore = balanceBefore,
            BalanceAfter = wallet.Balance,
            Reference = request.PaymentReference,
            Notes = $"Purchased {request.Credits} credits"
        };

        // Create a paid invoice for the purchase
        var invoice = new Invoice
        {
            ChurchId = churchId.Value,
            InvoiceNumber = $"INV-{dateTimeService.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}",
            Description = $"SMS Credits — {request.Credits:N0} credits",
            Amount = request.Amount,
            Currency = request.Currency,
            Status = InvoiceStatus.Paid,
            DueDate = dateTimeService.UtcNow,
            PaidAt = dateTimeService.UtcNow,
            PaymentMethod = request.PaymentMethod,
            PaymentReference = request.PaymentReference
        };

        smsCreditRepository.Update(wallet);
        await transactionRepository.AddAsync(transaction, cancellationToken);
        await invoiceRepository.AddAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<SmsCreditDto>.SuccessResult(new SmsCreditDto
        {
            Id = wallet.Id,
            ChurchId = wallet.ChurchId,
            Balance = wallet.Balance,
            TotalPurchased = wallet.TotalPurchased,
            TotalConsumed = wallet.TotalConsumed
        });
    }
}
