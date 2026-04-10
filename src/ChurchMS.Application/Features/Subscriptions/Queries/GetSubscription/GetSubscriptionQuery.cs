using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Queries.GetSubscription;

public record GetSubscriptionQuery : IRequest<ApiResponse<SubscriptionDto>>;
