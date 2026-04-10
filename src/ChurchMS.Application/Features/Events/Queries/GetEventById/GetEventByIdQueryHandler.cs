using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler(
    IRepository<ChurchEvent> eventRepository,
    IRepository<EventRegistration> registrationRepository,
    IRepository<EventAttendance> attendanceRepository)
    : IRequestHandler<GetEventByIdQuery, ApiResponse<EventDto>>
{
    public async Task<ApiResponse<EventDto>> Handle(
        GetEventByIdQuery request,
        CancellationToken cancellationToken)
    {
        var churchEvent = await eventRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(ChurchEvent), request.Id);

        var dto = churchEvent.Adapt<EventDto>();
        dto.RegistrationCount = await registrationRepository.CountAsync(
            r => r.EventId == request.Id && r.Status != Domain.Enums.RegistrationStatus.Cancelled,
            cancellationToken);
        dto.AttendanceCount = await attendanceRepository.CountAsync(
            a => a.EventId == request.Id, cancellationToken);

        return ApiResponse<EventDto>.SuccessResult(dto);
    }
}
