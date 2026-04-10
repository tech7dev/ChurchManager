using FluentValidation;

namespace ChurchMS.Application.Features.Messaging.Commands.CreateMessageCampaign;

public class CreateMessageCampaignValidator : AbstractValidator<CreateMessageCampaignCommand>
{
    public CreateMessageCampaignValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Body).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.Channel).IsInEnum();
        RuleFor(x => x.ScheduledAt)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ScheduledAt.HasValue)
            .WithMessage("Scheduled time must be in the future.");
    }
}
