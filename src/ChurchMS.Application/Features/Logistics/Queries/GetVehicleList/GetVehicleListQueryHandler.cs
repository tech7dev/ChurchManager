using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Queries.GetVehicleList;

public class GetVehicleListQueryHandler(
    IRepository<Vehicle> vehicleRepository)
    : IRequestHandler<GetVehicleListQuery, ApiResponse<PagedResult<VehicleDto>>>
{
    public async Task<ApiResponse<PagedResult<VehicleDto>>> Handle(
        GetVehicleListQuery request, CancellationToken cancellationToken)
    {
        var all = await vehicleRepository.FindAsync(
            v => !request.Status.HasValue || v.Status == request.Status.Value,
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderBy(v => v.Make).ThenBy(v => v.Model)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = paged.Select(v => new VehicleDto
        {
            Id = v.Id,
            ChurchId = v.ChurchId,
            Make = v.Make,
            Model = v.Model,
            Year = v.Year,
            PlateNumber = v.PlateNumber,
            Capacity = v.Capacity,
            Status = v.Status,
            Color = v.Color,
            Notes = v.Notes,
            CreatedAt = v.CreatedAt
        }).ToList();

        return ApiResponse<PagedResult<VehicleDto>>.SuccessResult(new PagedResult<VehicleDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
