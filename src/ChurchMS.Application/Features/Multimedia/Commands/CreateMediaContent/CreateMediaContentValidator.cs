using FluentValidation;

namespace ChurchMS.Application.Features.Multimedia.Commands.CreateMediaContent;

public class CreateMediaContentValidator : AbstractValidator<CreateMediaContentCommand>
{
    public CreateMediaContentValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
        RuleFor(x => x.Price).GreaterThan(0).When(x => x.Price.HasValue);
        RuleFor(x => x.Tags).MaximumLength(500).When(x => x.Tags is not null);
        RuleFor(x => x.FileUrl).MaximumLength(1000).When(x => x.FileUrl is not null);
        RuleFor(x => x.ThumbnailUrl).MaximumLength(1000).When(x => x.ThumbnailUrl is not null);
    }
}
