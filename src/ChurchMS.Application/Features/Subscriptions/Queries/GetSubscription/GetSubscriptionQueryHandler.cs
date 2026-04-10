using ChurchMS.Application.Features.Subscriptions.Commands.CreateSubscription;
using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Queries.GetSubscription;

public class GetSubscriptionQueryHandler(IRepository<Subscription> subscriptionRepository)
    : IRequestHandler<GetSubscriptionQuery, ApiResponse<SubscriptionDto>>
{
    public async Task<ApiResponse<SubscriptionDto>> Handle(
        GetSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var subscriptions = await subscriptionRepository.FindAsync(
            s => s.Status == SubscriptionStatus.Active
              || s.Status == SubscriptionStatus.Trial
              || s.Status == SubscriptionStatus.PastDue,
            cancellationToken);

        var active = subscriptions.OrderByDescending(s => s.CreatedAt).FirstOrDefault();

        if (active is null)
            return ApiResponse<SubscriptionDto>.FailureResult("No active subscription found.");

        return ApiResponse<SubscriptionDto>.SuccessResult(
            CreateSubscriptionCommandHandler.MapToDto(active));
    }
}
