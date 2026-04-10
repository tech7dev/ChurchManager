using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Commands.RecordAttendance;

public record RecordAttendanceCommand(
    Guid SessionId,
    Guid MemberId,
    AttendanceStatus Status,
    string? Notes) : IRequest<ApiResponse<GrowthAttendanceDto>>;
