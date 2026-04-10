using FluentValidation;

namespace ChurchMS.Application.Features.Secretariat.Commands.RecordMarriage;

public class RecordMarriageValidator : AbstractValidator<RecordMarriageCommand>
{
    public RecordMarriageValidator()
    {
        RuleFor(x => x.Spouse1MemberId).NotEmpty();
        RuleFor(x => x.MarriageDate).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Marriage date cannot be in the future.");
        RuleFor(x => x.Spouse2Name)
            .NotEmpty().WithMessage("Spouse 2 name is required when no member is linked.")
            .When(x => !x.Spouse2MemberId.HasValue);
        RuleFor(x => x.Location).MaximumLength(300).When(x => x.Location is not null);
        RuleFor(x => x.Notes).MaximumLength(2000).When(x => x.Notes is not null);
    }
}
