using FluentValidation;

namespace ChurchMS.Application.Features.SundaySchool.Commands.CreateClass;

public class CreateClassValidator : AbstractValidator<CreateClassCommand>
{
    public CreateClassValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Class name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
