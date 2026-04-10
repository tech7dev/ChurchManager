using FluentValidation;

namespace ChurchMS.Application.Features.Contributions.Commands.CreateFund;

public class CreateFundValidator : AbstractValidator<CreateFundCommand>
{
    public CreateFundValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Fund name is required.")
            .MaximumLength(100).WithMessage("Fund name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => x.Description is not null);
    }
}
