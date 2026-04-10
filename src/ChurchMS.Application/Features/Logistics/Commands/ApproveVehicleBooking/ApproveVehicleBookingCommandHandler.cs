using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Commands.ApproveVehicleBooking;

public class ApproveVehicleBookingCommandHandler(
    IRepository<VehicleBooking> bookingRepository,
    IRepository<Vehicle> vehicleRepository,
    IRepository<Member> memberRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ApproveVehicleBookingCommand, ApiResponse<VehicleBookingDto>>
{
    public async Task<ApiResponse<VehicleBookingDto>> Handle(
        ApproveVehicleBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByIdAsync(request.BookingId, cancellationToken)
            ?? throw new NotFoundException(nameof(VehicleBooking), request.BookingId);

        if (booking.Status != VehicleBookingStatus.Pending)
            return ApiResponse<VehicleBookingDto>.FailureResult("Only pending bookings can be approved.");

        booking.Status = VehicleBookingStatus.Approved;
        booking.ApprovedByMemberId = request.ApprovedByMemberId;
        booking.ApprovedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var vehicle = await vehicleRepository.GetByIdAsync(booking.VehicleId, cancellationToken);
        var approvedBy = await memberRepository.GetByIdAsync(request.ApprovedByMemberId, cancellationToken);

        string? driverName = null;
        if (booking.DriverMemberId.HasValue)
        {
            var driver = await memberRepository.GetByIdAsync(booking.DriverMemberId.Value, cancellationToken);
            driverName = driver is not null ? $"{driver.FirstName} {driver.LastName}" : null;
        }

        return ApiResponse<VehicleBookingDto>.SuccessResult(new VehicleBookingDto
        {
            Id = booking.Id,
            ChurchId = booking.ChurchId,
            VehicleId = booking.VehicleId,
            VehicleLabel = vehicle is not null
                ? $"{vehicle.Make} {vehicle.Model}" + (vehicle.PlateNumber is not null ? $" ({vehicle.PlateNumber})" : "")
                : "",
            DriverMemberId = booking.DriverMemberId,
            DriverName = driverName,
            RelatedEventId = booking.RelatedEventId,
            Purpose = booking.Purpose,
            StartDateTime = booking.StartDateTime,
            EndDateTime = booking.EndDateTime,
            Status = booking.Status,
            ApprovedByMemberId = booking.ApprovedByMemberId,
            ApprovedByName = approvedBy is not null ? $"{approvedBy.FirstName} {approvedBy.LastName}" : null,
            ApprovedAt = booking.ApprovedAt,
            Notes = booking.Notes,
            CreatedAt = booking.CreatedAt
        });
    }
}
