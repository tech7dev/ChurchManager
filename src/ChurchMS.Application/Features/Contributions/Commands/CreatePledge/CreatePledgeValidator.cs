using FluentValidation;

namespace ChurchMS.Application.Features.Contributions.Commands.CreatePledge;

public class CreatePledgeValidator : AbstractValidator<CreatePledgeCommand>
{
    public CreatePledgeValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member is required.");

        RuleFor(x => x.FundId)
            .NotEmpty().WithMessage("Fund is required.");

        RuleFor(x => x.PledgedAmount)
            .GreaterThan(0).WithMessage("Pledged amount must be greater than zero.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");
    }
}
