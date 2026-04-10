using FluentValidation;

namespace ChurchMS.Application.Features.Logistics.Commands.BookVehicle;

public class BookVehicleValidator : AbstractValidator<BookVehicleCommand>
{
    public BookVehicleValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.Purpose).NotEmpty().MaximumLength(300);
        RuleFor(x => x.StartDateTime).NotEmpty();
        RuleFor(x => x.EndDateTime).GreaterThan(x => x.StartDateTime)
            .WithMessage("End time must be after start time.");
    }
}
