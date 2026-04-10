using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetAttendanceReport;

public record GetAttendanceReportQuery(
    DateTime From,
    DateTime To
) : IRequest<ApiResponse<AttendanceReportDto>>;
