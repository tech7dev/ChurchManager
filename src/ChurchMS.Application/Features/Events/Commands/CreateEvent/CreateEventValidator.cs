using FluentValidation;

namespace ChurchMS.Application.Features.Events.Commands.CreateEvent;

public class CreateEventValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Event title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.StartDateTime)
            .NotEmpty().WithMessage("Start date/time is required.");

        RuleFor(x => x.EndDateTime)
            .NotEmpty().WithMessage("End date/time is required.")
            .GreaterThan(x => x.StartDateTime).WithMessage("End must be after start.");

        RuleFor(x => x.MaxAttendees)
            .GreaterThan(0).WithMessage("Max attendees must be positive.")
            .When(x => x.MaxAttendees.HasValue);

        RuleFor(x => x.RegistrationFee)
            .GreaterThanOrEqualTo(0).WithMessage("Fee cannot be negative.")
            .When(x => x.RegistrationFee.HasValue);

        RuleFor(x => x.RecurrenceFrequency)
            .NotNull().WithMessage("Recurrence frequency is required for recurring events.")
            .When(x => x.IsRecurring);
    }
}
