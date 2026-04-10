using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Events.Commands.RecordAttendance;

public record RecordAttendanceCommand(
    Guid EventId,
    Guid? MemberId,
    string? VisitorName,
    DateOnly AttendanceDate,
    AttendanceStatus Status,
    bool RecordedByQr,
    string? Notes) : IRequest<ApiResponse<EventAttendanceDto>>;
