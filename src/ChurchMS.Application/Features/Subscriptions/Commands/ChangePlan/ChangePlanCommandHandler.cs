using ChurchMS.Application.Features.Subscriptions.Commands.CreateSubscription;
using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.ChangePlan;

public class ChangePlanCommandHandler(
    IRepository<Subscription> subscriptionRepository,
    IRepository<Church> churchRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangePlanCommand, ApiResponse<SubscriptionDto>>
{
    public async Task<ApiResponse<SubscriptionDto>> Handle(
        ChangePlanCommand request, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.GetByIdAsync(
            request.SubscriptionId, cancellationToken);
        if (subscription is null)
            return ApiResponse<SubscriptionDto>.FailureResult("Subscription not found.");

        subscription.Plan = request.NewPlan;
        subscription.Amount = request.NewAmount;

        // Update church's plan enum
        var church = await churchRepository.GetByIdAsync(subscription.ChurchId, cancellationToken);
        if (church is not null)
        {
            church.SubscriptionPlan = request.NewPlan;
            churchRepository.Update(church);
        }

        subscriptionRepository.Update(subscription);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<SubscriptionDto>.SuccessResult(
            CreateSubscriptionCommandHandler.MapToDto(subscription));
    }
}
