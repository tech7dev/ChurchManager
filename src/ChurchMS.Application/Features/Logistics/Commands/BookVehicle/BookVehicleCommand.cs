using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Commands.BookVehicle;

public record BookVehicleCommand(
    Guid VehicleId,
    Guid? DriverMemberId,
    Guid? RelatedEventId,
    string Purpose,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string? Notes
) : IRequest<ApiResponse<VehicleBookingDto>>;
