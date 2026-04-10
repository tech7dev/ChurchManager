using FluentValidation;

namespace ChurchMS.Application.Features.Multimedia.Commands.CreatePromotion;

public class CreatePromotionValidator : AbstractValidator<CreatePromotionCommand>
{
    public CreatePromotionValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000).When(x => x.Description is not null);
        RuleFor(x => x.Code).MaximumLength(50).When(x => x.Code is not null);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(1, 100).When(x => x.DiscountPercent.HasValue);
        RuleFor(x => x.DiscountAmount).GreaterThan(0).When(x => x.DiscountAmount.HasValue);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");
    }
}
