using FluentValidation;

namespace ChurchMS.Application.Features.Subscriptions.Commands.PurchaseSmsCredits;

public class PurchaseSmsCreditsValidator : AbstractValidator<PurchaseSmsCreditsCommand>
{
    public PurchaseSmsCreditsValidator()
    {
        RuleFor(x => x.Credits).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.PaymentMethod).IsInEnum();
    }
}
