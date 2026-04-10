using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Commands.CreateVehicle;

public class CreateVehicleCommandHandler(
    IRepository<Vehicle> vehicleRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateVehicleCommand, ApiResponse<VehicleDto>>
{
    public async Task<ApiResponse<VehicleDto>> Handle(
        CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<VehicleDto>.FailureResult("Church context required.");

        var vehicle = new Vehicle
        {
            ChurchId = churchId.Value,
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            PlateNumber = request.PlateNumber,
            Capacity = request.Capacity,
            Color = request.Color,
            Notes = request.Notes,
            Status = VehicleStatus.Available
        };

        await vehicleRepository.AddAsync(vehicle, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<VehicleDto>.SuccessResult(new VehicleDto
        {
            Id = vehicle.Id,
            ChurchId = vehicle.ChurchId,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            PlateNumber = vehicle.PlateNumber,
            Capacity = vehicle.Capacity,
            Status = vehicle.Status,
            Color = vehicle.Color,
            Notes = vehicle.Notes,
            CreatedAt = vehicle.CreatedAt
        });
    }
}
