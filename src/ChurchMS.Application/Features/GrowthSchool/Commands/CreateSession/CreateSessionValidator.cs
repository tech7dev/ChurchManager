using FluentValidation;

namespace ChurchMS.Application.Features.GrowthSchool.Commands.CreateSession;

public class CreateSessionValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty().WithMessage("Course is required.");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Session title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
        RuleFor(x => x.SessionDate).NotEmpty().WithMessage("Session date is required.");
    }
}
