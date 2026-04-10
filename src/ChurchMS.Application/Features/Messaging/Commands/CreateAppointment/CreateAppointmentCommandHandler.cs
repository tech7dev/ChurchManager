using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Application.Exceptions;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler(
    IRepository<Appointment> appointmentRepository,
    IRepository<Member> memberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAppointmentCommand, ApiResponse<AppointmentDto>>
{
    public async Task<ApiResponse<AppointmentDto>> Handle(
        CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()!.Value;

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var responsible = await memberRepository.GetByIdAsync(request.ResponsibleMemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.ResponsibleMemberId);

        var appointment = new Appointment
        {
            ChurchId = churchId,
            MemberId = request.MemberId,
            ResponsibleMemberId = request.ResponsibleMemberId,
            Subject = request.Subject,
            Description = request.Description,
            MeetingType = request.MeetingType,
            Location = request.Location,
            Status = AppointmentStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };

        await appointmentRepository.AddAsync(appointment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<AppointmentDto>.SuccessResult(new AppointmentDto
        {
            Id = appointment.Id,
            ChurchId = appointment.ChurchId,
            MemberId = appointment.MemberId,
            MemberName = $"{member.FirstName} {member.LastName}",
            ResponsibleMemberId = appointment.ResponsibleMemberId,
            ResponsibleName = $"{responsible.FirstName} {responsible.LastName}",
            Subject = appointment.Subject,
            Description = appointment.Description,
            Status = appointment.Status,
            MeetingType = appointment.MeetingType,
            RequestedAt = appointment.RequestedAt,
            Location = appointment.Location,
            CreatedAt = appointment.CreatedAt
        });
    }
}
