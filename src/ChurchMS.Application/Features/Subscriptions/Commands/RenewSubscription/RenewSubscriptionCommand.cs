using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.RenewSubscription;

public record RenewSubscriptionCommand(
    Guid SubscriptionId,
    PaymentMethod PaymentMethod,
    string? PaymentReference
) : IRequest<ApiResponse<SubscriptionDto>>;
