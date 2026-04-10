using FluentValidation;

namespace ChurchMS.Application.Features.Contributions.Commands.CreateContribution;

public class CreateContributionValidator : AbstractValidator<CreateContributionCommand>
{
    public CreateContributionValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.");

        RuleFor(x => x.ContributionDate)
            .NotEmpty().WithMessage("Contribution date is required.");

        RuleFor(x => x.FundId)
            .NotEmpty().WithMessage("Fund is required.");

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member ID is required when no anonymous donor name is provided.")
            .When(x => string.IsNullOrWhiteSpace(x.AnonymousDonorName));

        RuleFor(x => x.RecurrenceFrequency)
            .NotNull().WithMessage("Recurrence frequency is required for recurring contributions.")
            .When(x => x.IsRecurring);

        RuleFor(x => x.RecurrenceEndDate)
            .NotNull().WithMessage("Recurrence end date is required for recurring contributions.")
            .When(x => x.IsRecurring);
    }
}
