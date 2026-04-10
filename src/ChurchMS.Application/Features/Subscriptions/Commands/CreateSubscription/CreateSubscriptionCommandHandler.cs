using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler(
    IRepository<Subscription> subscriptionRepository,
    IRepository<Church> churchRepository,
    IRepository<Invoice> invoiceRepository,
    ITenantService tenantService,
    IDateTimeService dateTimeService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSubscriptionCommand, ApiResponse<SubscriptionDto>>
{
    public async Task<ApiResponse<SubscriptionDto>> Handle(
        CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<SubscriptionDto>.FailureResult("Church context required.");

        // Check for existing active subscription
        var existing = await subscriptionRepository.FindAsync(
            s => s.ChurchId == churchId.Value
              && (s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.Trial),
            cancellationToken);

        if (existing.Count > 0)
            return ApiResponse<SubscriptionDto>.FailureResult(
                "Church already has an active subscription.");

        var now = dateTimeService.UtcNow;
        var trialEnd = request.TrialDays > 0 ? now.AddDays(request.TrialDays) : (DateTime?)null;
        var subscriptionEnd = request.BillingCycle == BillingCycle.Annual
            ? now.AddYears(1)
            : now.AddMonths(1);

        var subscription = new Subscription
        {
            ChurchId = churchId.Value,
            Plan = request.Plan,
            Status = request.TrialDays > 0 ? SubscriptionStatus.Trial : SubscriptionStatus.Active,
            BillingCycle = request.BillingCycle,
            Amount = request.Amount,
            Currency = request.Currency,
            AutoRenew = request.AutoRenew,
            StartDate = now,
            EndDate = subscriptionEnd,
            TrialEndDate = trialEnd,
            NextBillingDate = trialEnd ?? subscriptionEnd,
            PaymentMethod = request.PaymentMethod,
            ExternalSubscriptionId = request.ExternalSubscriptionId
        };

        await subscriptionRepository.AddAsync(subscription, cancellationToken);

        // Update church's plan enum
        var church = await churchRepository.GetByIdAsync(churchId.Value, cancellationToken);
        if (church is not null)
        {
            church.SubscriptionPlan = request.Plan;
            churchRepository.Update(church);
        }

        // Generate invoice if not a free trial
        if (request.Amount > 0 && request.TrialDays == 0)
        {
            var invoice = new Invoice
            {
                ChurchId = churchId.Value,
                InvoiceNumber = GenerateInvoiceNumber(),
                Description = $"{request.Plan} plan — {request.BillingCycle} subscription",
                Amount = request.Amount,
                Currency = request.Currency,
                Status = InvoiceStatus.Sent,
                DueDate = now.AddDays(7),
                SubscriptionId = subscription.Id
            };
            await invoiceRepository.AddAsync(invoice, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<SubscriptionDto>.SuccessResult(MapToDto(subscription));
    }

    private static string GenerateInvoiceNumber()
        => $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

    internal static SubscriptionDto MapToDto(Subscription s) => new()
    {
        Id = s.Id,
        ChurchId = s.ChurchId,
        Plan = s.Plan,
        Status = s.Status,
        BillingCycle = s.BillingCycle,
        Amount = s.Amount,
        Currency = s.Currency,
        AutoRenew = s.AutoRenew,
        StartDate = s.StartDate,
        EndDate = s.EndDate,
        TrialEndDate = s.TrialEndDate,
        LastPaymentDate = s.LastPaymentDate,
        NextBillingDate = s.NextBillingDate,
        CancellationDate = s.CancellationDate,
        CancellationReason = s.CancellationReason,
        PaymentMethod = s.PaymentMethod,
        ExternalSubscriptionId = s.ExternalSubscriptionId,
        CreatedAt = s.CreatedAt,
        UpdatedAt = s.UpdatedAt
    };
}
