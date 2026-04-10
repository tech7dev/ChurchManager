using FluentValidation;

namespace ChurchMS.Application.Features.Secretariat.Commands.RegisterDocument;

public class RegisterDocumentValidator : AbstractValidator<RegisterDocumentCommand>
{
    public RegisterDocumentValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(300);
        RuleFor(x => x.FileUrl).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.FileSize).GreaterThan(0);
        RuleFor(x => x.Type).IsInEnum();
    }
}
