using FluentValidation;

namespace ChurchMS.Application.Features.Secretariat.Commands.RecordBaptism;

public class RecordBaptismValidator : AbstractValidator<RecordBaptismCommand>
{
    public RecordBaptismValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty();
        RuleFor(x => x.BaptismDate).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Baptism date cannot be in the future.");
        RuleFor(x => x.Location).MaximumLength(300).When(x => x.Location is not null);
        RuleFor(x => x.Notes).MaximumLength(2000).When(x => x.Notes is not null);
    }
}
