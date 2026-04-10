using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Commands.CreateVehicle;

public record CreateVehicleCommand(
    string Make,
    string Model,
    int? Year,
    string? PlateNumber,
    int? Capacity,
    string? Color,
    string? Notes
) : IRequest<ApiResponse<VehicleDto>>;
