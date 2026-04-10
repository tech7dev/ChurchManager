using FluentValidation;

namespace ChurchMS.Application.Features.Accounting.Commands.RecordTransaction;

public class RecordTransactionValidator : AbstractValidator<RecordTransactionCommand>
{
    public RecordTransactionValidator()
    {
        RuleFor(x => x.BankAccountId)
            .NotEmpty().WithMessage("Bank account is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction date is required.");
    }
}
