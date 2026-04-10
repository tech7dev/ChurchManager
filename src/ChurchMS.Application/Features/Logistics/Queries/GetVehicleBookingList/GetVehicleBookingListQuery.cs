using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Queries.GetVehicleBookingList;

public record GetVehicleBookingListQuery(
    Guid? VehicleId = null,
    VehicleBookingStatus? Status = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<VehicleBookingDto>>>;
