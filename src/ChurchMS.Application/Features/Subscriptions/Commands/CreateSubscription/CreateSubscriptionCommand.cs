using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(
    SubscriptionPlan Plan,
    BillingCycle BillingCycle,
    decimal Amount,
    string Currency,
    bool AutoRenew,
    int TrialDays,
    PaymentMethod? PaymentMethod,
    string? ExternalSubscriptionId
) : IRequest<ApiResponse<SubscriptionDto>>;
