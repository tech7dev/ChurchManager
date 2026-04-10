using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.CancelSubscription;

public record CancelSubscriptionCommand(
    Guid SubscriptionId,
    string? Reason
) : IRequest<ApiResponse<SubscriptionDto>>;
