using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.ChangePlan;

public record ChangePlanCommand(
    Guid SubscriptionId,
    SubscriptionPlan NewPlan,
    decimal NewAmount
) : IRequest<ApiResponse<SubscriptionDto>>;
