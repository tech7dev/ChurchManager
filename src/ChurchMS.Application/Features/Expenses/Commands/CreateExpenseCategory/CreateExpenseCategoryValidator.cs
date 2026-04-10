using FluentValidation;

namespace ChurchMS.Application.Features.Expenses.Commands.CreateExpenseCategory;

public class CreateExpenseCategoryValidator : AbstractValidator<CreateExpenseCategoryCommand>
{
    public CreateExpenseCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
