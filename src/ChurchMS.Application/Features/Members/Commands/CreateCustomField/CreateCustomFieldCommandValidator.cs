using FluentValidation;

namespace ChurchMS.Application.Features.Members.Commands.CreateCustomField;

public class CreateCustomFieldCommandValidator : AbstractValidator<CreateCustomFieldCommand>
{
    public CreateCustomFieldCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Field name is required.")
            .MaximumLength(100);

        RuleFor(x => x.FieldType)
            .IsInEnum().WithMessage("Invalid field type.");
    }
}
