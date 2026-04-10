using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Commands.CreateAppointment;

public record CreateAppointmentCommand(
    Guid MemberId,
    Guid ResponsibleMemberId,
    string Subject,
    string? Description,
    MeetingType MeetingType,
    string? Location
) : IRequest<ApiResponse<AppointmentDto>>;
