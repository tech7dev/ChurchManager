using FluentValidation;

namespace ChurchMS.Application.Features.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionValidator()
    {
        RuleFor(x => x.Plan).IsInEnum();
        RuleFor(x => x.BillingCycle).IsInEnum();
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.TrialDays).GreaterThanOrEqualTo(0);
    }
}
