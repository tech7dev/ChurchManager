using FluentValidation;

namespace ChurchMS.Application.Features.Messaging.Commands.CreateAppointment;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.MemberId).NotEmpty();
        RuleFor(x => x.ResponsibleMemberId).NotEmpty();
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
        RuleFor(x => x.MeetingType).IsInEnum();
    }
}
