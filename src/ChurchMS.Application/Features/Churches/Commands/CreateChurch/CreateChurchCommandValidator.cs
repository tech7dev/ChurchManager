using FluentValidation;

namespace ChurchMS.Application.Features.Churches.Commands.CreateChurch;

public class CreateChurchCommandValidator : AbstractValidator<CreateChurchCommand>
{
    public CreateChurchCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Church name is required.")
            .MaximumLength(200);

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Church code is required.")
            .MaximumLength(20)
            .Matches("^[A-Za-z0-9-]+$").WithMessage("Church code can only contain letters, numbers, and hyphens.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(100);

        RuleFor(x => x.PrimaryCurrency)
            .NotEmpty().WithMessage("Primary currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.");

        RuleFor(x => x.SecondaryCurrency)
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.")
            .When(x => !string.IsNullOrEmpty(x.SecondaryCurrency));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("A valid email address is required.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.TimeZone)
            .NotEmpty().WithMessage("TimeZone is required.");
    }
}
