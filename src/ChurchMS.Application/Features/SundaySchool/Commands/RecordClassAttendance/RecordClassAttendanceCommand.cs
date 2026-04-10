using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Commands.RecordClassAttendance;

public record RecordClassAttendanceCommand(
    Guid LessonId,
    Guid MemberId,
    AttendanceStatus Status,
    string? Notes) : IRequest<ApiResponse<ClassAttendanceDto>>;
