using FluentValidation;

namespace ChurchMS.Application.Features.GrowthSchool.Commands.CreateCourse;

public class CreateCourseValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Course name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
