using FluentValidation;

namespace ChurchMS.Application.Features.SundaySchool.Commands.CreateLesson;

public class CreateLessonValidator : AbstractValidator<CreateLessonCommand>
{
    public CreateLessonValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty().WithMessage("Class is required.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Lesson title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
        RuleFor(x => x.LessonDate).NotEmpty().WithMessage("Lesson date is required.");
    }
}
