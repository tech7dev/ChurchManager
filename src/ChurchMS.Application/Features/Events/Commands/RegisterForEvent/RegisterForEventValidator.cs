using FluentValidation;

namespace ChurchMS.Application.Features.Events.Commands.RegisterForEvent;

public class RegisterForEventValidator : AbstractValidator<RegisterForEventCommand>
{
    public RegisterForEventValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("Event is required.");

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member ID or guest name is required.")
            .When(x => string.IsNullOrWhiteSpace(x.GuestName));

        RuleFor(x => x.GuestName)
            .NotEmpty().WithMessage("Guest name or member ID is required.")
            .When(x => !x.MemberId.HasValue);
    }
}
