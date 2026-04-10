using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Commands.BookVehicle;

public class BookVehicleCommandHandler(
    IRepository<Vehicle> vehicleRepository,
    IRepository<VehicleBooking> bookingRepository,
    IRepository<Member> memberRepository,
    IRepository<ChurchEvent> eventRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<BookVehicleCommand, ApiResponse<VehicleBookingDto>>
{
    public async Task<ApiResponse<VehicleBookingDto>> Handle(
        BookVehicleCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<VehicleBookingDto>.FailureResult("Church context required.");

        if (request.EndDateTime <= request.StartDateTime)
            return ApiResponse<VehicleBookingDto>.FailureResult("End time must be after start time.");

        var vehicle = await vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken)
            ?? throw new NotFoundException(nameof(Vehicle), request.VehicleId);

        if (vehicle.Status == VehicleStatus.Retired)
            return ApiResponse<VehicleBookingDto>.FailureResult("Vehicle is retired and cannot be booked.");

        // Check for overlapping active bookings
        var overlapping = await bookingRepository.FindAsync(
            b => b.VehicleId == request.VehicleId
              && (b.Status == VehicleBookingStatus.Approved || b.Status == VehicleBookingStatus.InProgress)
              && b.StartDateTime < request.EndDateTime
              && b.EndDateTime > request.StartDateTime,
            cancellationToken);

        if (overlapping.Count > 0)
            return ApiResponse<VehicleBookingDto>.FailureResult("Vehicle is already booked for this time slot.");

        var booking = new VehicleBooking
        {
            ChurchId = churchId.Value,
            VehicleId = vehicle.Id,
            DriverMemberId = request.DriverMemberId,
            RelatedEventId = request.RelatedEventId,
            Purpose = request.Purpose,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            Status = VehicleBookingStatus.Pending,
            Notes = request.Notes
        };

        await bookingRepository.AddAsync(booking, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? driverName = null;
        if (booking.DriverMemberId.HasValue)
        {
            var driver = await memberRepository.GetByIdAsync(booking.DriverMemberId.Value, cancellationToken);
            driverName = driver is not null ? $"{driver.FirstName} {driver.LastName}" : null;
        }

        string? eventTitle = null;
        if (booking.RelatedEventId.HasValue)
        {
            var ev = await eventRepository.GetByIdAsync(booking.RelatedEventId.Value, cancellationToken);
            eventTitle = ev?.Title;
        }

        return ApiResponse<VehicleBookingDto>.SuccessResult(new VehicleBookingDto
        {
            Id = booking.Id,
            ChurchId = booking.ChurchId,
            VehicleId = booking.VehicleId,
            VehicleLabel = $"{vehicle.Make} {vehicle.Model}" + (vehicle.PlateNumber is not null ? $" ({vehicle.PlateNumber})" : ""),
            DriverMemberId = booking.DriverMemberId,
            DriverName = driverName,
            RelatedEventId = booking.RelatedEventId,
            RelatedEventTitle = eventTitle,
            Purpose = booking.Purpose,
            StartDateTime = booking.StartDateTime,
            EndDateTime = booking.EndDateTime,
            Status = booking.Status,
            Notes = booking.Notes,
            CreatedAt = booking.CreatedAt
        });
    }
}
