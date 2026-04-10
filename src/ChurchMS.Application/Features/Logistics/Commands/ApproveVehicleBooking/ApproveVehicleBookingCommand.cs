using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Commands.ApproveVehicleBooking;

public record ApproveVehicleBookingCommand(
    Guid BookingId,
    Guid ApprovedByMemberId
) : IRequest<ApiResponse<VehicleBookingDto>>;
