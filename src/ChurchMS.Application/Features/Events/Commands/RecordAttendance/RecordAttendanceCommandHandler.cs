using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Events.Commands.RecordAttendance;

public class RecordAttendanceCommandHandler(
    IRepository<EventAttendance> attendanceRepository,
    IRepository<ChurchEvent> eventRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<RecordAttendanceCommand, ApiResponse<EventAttendanceDto>>
{
    public async Task<ApiResponse<EventAttendanceDto>> Handle(
        RecordAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var churchEvent = await eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(ChurchEvent), request.EventId);

        var attendance = new EventAttendance
        {
            ChurchId = churchId,
            EventId = request.EventId,
            MemberId = request.MemberId,
            VisitorName = request.VisitorName,
            AttendanceDate = request.AttendanceDate,
            Status = request.Status,
            RecordedByQr = request.RecordedByQr,
            Notes = request.Notes
        };

        await attendanceRepository.AddAsync(attendance, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<EventAttendanceDto>.SuccessResult(
            attendance.Adapt<EventAttendanceDto>(), "Attendance recorded.");
    }
}
