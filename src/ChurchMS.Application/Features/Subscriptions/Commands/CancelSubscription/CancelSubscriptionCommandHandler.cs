using ChurchMS.Application.Features.Subscriptions.Commands.CreateSubscription;
using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionCommandHandler(
    IRepository<Subscription> subscriptionRepository,
    IDateTimeService dateTimeService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CancelSubscriptionCommand, ApiResponse<SubscriptionDto>>
{
    public async Task<ApiResponse<SubscriptionDto>> Handle(
        CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.GetByIdAsync(
            request.SubscriptionId, cancellationToken);
        if (subscription is null)
            return ApiResponse<SubscriptionDto>.FailureResult("Subscription not found.");

        if (subscription.Status == SubscriptionStatus.Cancelled)
            return ApiResponse<SubscriptionDto>.FailureResult("Subscription is already cancelled.");

        subscription.Status = SubscriptionStatus.Cancelled;
        subscription.AutoRenew = false;
        subscription.CancellationDate = dateTimeService.UtcNow;
        subscription.CancellationReason = request.Reason;

        subscriptionRepository.Update(subscription);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<SubscriptionDto>.SuccessResult(
            CreateSubscriptionCommandHandler.MapToDto(subscription));
    }
}
