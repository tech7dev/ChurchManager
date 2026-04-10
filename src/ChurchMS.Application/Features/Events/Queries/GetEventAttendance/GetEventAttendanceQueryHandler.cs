using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Events.Queries.GetEventAttendance;

public class GetEventAttendanceQueryHandler(
    IRepository<EventAttendance> attendanceRepository,
    IRepository<ChurchEvent> eventRepository)
    : IRequestHandler<GetEventAttendanceQuery, ApiResponse<PagedResult<EventAttendanceDto>>>
{
    public async Task<ApiResponse<PagedResult<EventAttendanceDto>>> Handle(
        GetEventAttendanceQuery request,
        CancellationToken cancellationToken)
    {
        _ = await eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(ChurchEvent), request.EventId);

        var all = await attendanceRepository.FindAsync(a =>
            a.EventId == request.EventId &&
            (!request.Date.HasValue || a.AttendanceDate == request.Date.Value),
            cancellationToken);

        var totalCount = all.Count;
        var items = all
            .OrderByDescending(a => a.AttendanceDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Adapt<List<EventAttendanceDto>>();

        var result = new PagedResult<EventAttendanceDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
        return ApiResponse<PagedResult<EventAttendanceDto>>.SuccessResult(result);
    }
}
