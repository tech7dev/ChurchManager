using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Logistics.Queries.GetVehicleBookingList;

public class GetVehicleBookingListQueryHandler(
    IRepository<VehicleBooking> bookingRepository,
    IRepository<Vehicle> vehicleRepository,
    IRepository<Member> memberRepository,
    IRepository<ChurchEvent> eventRepository)
    : IRequestHandler<GetVehicleBookingListQuery, ApiResponse<PagedResult<VehicleBookingDto>>>
{
    public async Task<ApiResponse<PagedResult<VehicleBookingDto>>> Handle(
        GetVehicleBookingListQuery request, CancellationToken cancellationToken)
    {
        var all = await bookingRepository.FindAsync(
            b => (!request.VehicleId.HasValue || b.VehicleId == request.VehicleId.Value)
              && (!request.Status.HasValue || b.Status == request.Status.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(b => b.StartDateTime)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<VehicleBookingDto>();
        foreach (var booking in paged)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(booking.VehicleId, cancellationToken);

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

            string? approvedByName = null;
            if (booking.ApprovedByMemberId.HasValue)
            {
                var approver = await memberRepository.GetByIdAsync(booking.ApprovedByMemberId.Value, cancellationToken);
                approvedByName = approver is not null ? $"{approver.FirstName} {approver.LastName}" : null;
            }

            dtos.Add(new VehicleBookingDto
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
                RelatedEventTitle = eventTitle,
                Purpose = booking.Purpose,
                StartDateTime = booking.StartDateTime,
                EndDateTime = booking.EndDateTime,
                Status = booking.Status,
                ApprovedByMemberId = booking.ApprovedByMemberId,
                ApprovedByName = approvedByName,
                ApprovedAt = booking.ApprovedAt,
                Notes = booking.Notes,
                CreatedAt = booking.CreatedAt
            });
        }

        return ApiResponse<PagedResult<VehicleBookingDto>>.SuccessResult(new PagedResult<VehicleBookingDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
