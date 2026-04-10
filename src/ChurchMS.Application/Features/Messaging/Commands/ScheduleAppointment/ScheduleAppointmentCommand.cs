using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Commands.ScheduleAppointment;

public record ScheduleAppointmentCommand(
    Guid AppointmentId,
    DateTime ScheduledAt,
    string? VideoCallLink,
    string? Location,
    string? Notes
) : IRequest<ApiResponse<AppointmentDto>>;
