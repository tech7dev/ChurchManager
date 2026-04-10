using ChurchMS.Application.Features.Subscriptions.Commands.CreateSubscription;
using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.RenewSubscription;

public class RenewSubscriptionCommandHandler(
    IRepository<Subscription> subscriptionRepository,
    IRepository<Invoice> invoiceRepository,
    IDateTimeService dateTimeService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RenewSubscriptionCommand, ApiResponse<SubscriptionDto>>
{
    public async Task<ApiResponse<SubscriptionDto>> Handle(
        RenewSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.GetByIdAsync(
            request.SubscriptionId, cancellationToken);
        if (subscription is null)
            return ApiResponse<SubscriptionDto>.FailureResult("Subscription not found.");

        var now = dateTimeService.UtcNow;
        var newEnd = subscription.BillingCycle == BillingCycle.Annual
            ? subscription.EndDate.AddYears(1)
            : subscription.EndDate.AddMonths(1);

        subscription.Status = SubscriptionStatus.Active;
        subscription.EndDate = newEnd;
        subscription.LastPaymentDate = now;
        subscription.NextBillingDate = newEnd;
        subscription.PaymentMethod = request.PaymentMethod;

        // Create paid invoice for the renewal
        var invoice = new Invoice
        {
            ChurchId = subscription.ChurchId,
            InvoiceNumber = $"INV-{now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}",
            Description = $"{subscription.Plan} plan renewal — {subscription.BillingCycle}",
            Amount = subscription.Amount,
            Currency = subscription.Currency,
            Status = InvoiceStatus.Paid,
            DueDate = now,
            PaidAt = now,
            PaymentMethod = request.PaymentMethod,
            PaymentReference = request.PaymentReference,
            SubscriptionId = subscription.Id
        };

        subscriptionRepository.Update(subscription);
        await invoiceRepository.AddAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<SubscriptionDto>.SuccessResult(
            CreateSubscriptionCommandHandler.MapToDto(subscription));
    }
}
