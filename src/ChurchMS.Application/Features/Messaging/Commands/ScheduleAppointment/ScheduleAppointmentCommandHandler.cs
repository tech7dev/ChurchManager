using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Application.Exceptions;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Commands.ScheduleAppointment;

public class ScheduleAppointmentCommandHandler(
    IRepository<Appointment> appointmentRepository,
    IRepository<Member> memberRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ScheduleAppointmentCommand, ApiResponse<AppointmentDto>>
{
    public async Task<ApiResponse<AppointmentDto>> Handle(
        ScheduleAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetByIdAsync(request.AppointmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Appointment), request.AppointmentId);

        if (appointment.Status != AppointmentStatus.Pending)
            return ApiResponse<AppointmentDto>.FailureResult("Only pending appointments can be scheduled.");

        appointment.Status = AppointmentStatus.Scheduled;
        appointment.ScheduledAt = request.ScheduledAt;
        appointment.VideoCallLink = request.VideoCallLink;
        appointment.Location = request.Location;
        appointment.Notes = request.Notes;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var member = await memberRepository.GetByIdAsync(appointment.MemberId, cancellationToken);
        var responsible = await memberRepository.GetByIdAsync(appointment.ResponsibleMemberId, cancellationToken);

        return ApiResponse<AppointmentDto>.SuccessResult(new AppointmentDto
        {
            Id = appointment.Id,
            ChurchId = appointment.ChurchId,
            MemberId = appointment.MemberId,
            MemberName = member is not null ? $"{member.FirstName} {member.LastName}" : "",
            ResponsibleMemberId = appointment.ResponsibleMemberId,
            ResponsibleName = responsible is not null ? $"{responsible.FirstName} {responsible.LastName}" : "",
            Subject = appointment.Subject,
            Description = appointment.Description,
            Status = appointment.Status,
            MeetingType = appointment.MeetingType,
            RequestedAt = appointment.RequestedAt,
            ScheduledAt = appointment.ScheduledAt,
            VideoCallLink = appointment.VideoCallLink,
            Location = appointment.Location,
            Notes = appointment.Notes,
            CreatedAt = appointment.CreatedAt
        });
    }
}
