using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Events.Queries.GetEventAttendance;

public record GetEventAttendanceQuery(
    Guid EventId,
    DateOnly? Date = null,
    int Page = 1,
    int PageSize = 100) : IRequest<ApiResponse<PagedResult<EventAttendanceDto>>>;
