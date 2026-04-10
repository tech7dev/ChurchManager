using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetAttendanceReport;

public class GetAttendanceReportQueryHandler(
    IRepository<ChurchEvent> eventRepository,
    IRepository<EventRegistration> registrationRepository,
    IRepository<EventAttendance> attendanceRepository)
    : IRequestHandler<GetAttendanceReportQuery, ApiResponse<AttendanceReportDto>>
{
    public async Task<ApiResponse<AttendanceReportDto>> Handle(
        GetAttendanceReportQuery request, CancellationToken cancellationToken)
    {
        var events = await eventRepository.FindAsync(
            e => e.StartDateTime >= request.From && e.StartDateTime <= request.To,
            cancellationToken);

        var eventIds = events.Select(e => e.Id).ToHashSet();

        var registrations = await registrationRepository.FindAsync(
            r => eventIds.Contains(r.EventId), cancellationToken);

        var attendance = await attendanceRepository.FindAsync(
            a => eventIds.Contains(a.EventId), cancellationToken);

        // Per-event breakdown
        var registrationsByEvent = registrations.GroupBy(r => r.EventId)
            .ToDictionary(g => g.Key, g => g.Count());
        var attendedByEvent = attendance
            .Where(a => a.Status == AttendanceStatus.Present)
            .GroupBy(a => a.EventId)
            .ToDictionary(g => g.Key, g => g.Count());

        var byEvent = events
            .OrderByDescending(e => e.StartDateTime)
            .Select(e =>
            {
                var registered = registrationsByEvent.GetValueOrDefault(e.Id, 0);
                var attended = attendedByEvent.GetValueOrDefault(e.Id, 0);
                return new EventAttendanceSummaryDto
                {
                    EventName = e.Title,
                    EventDate = e.StartDateTime,
                    Registered = registered,
                    Attended = attended,
                    AttendanceRate = registered > 0 ? Math.Round((double)attended / registered * 100, 1) : 0
                };
            })
            .ToList();

        // Monthly trend
        var allMonths = new List<MonthlyAttendanceTrendDto>();
        var cursor = new DateTime(request.From.Year, request.From.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(request.To.Year, request.To.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        while (cursor <= end)
        {
            var eventIdsThisMonth = events
                .Where(e => e.StartDateTime.Year == cursor.Year && e.StartDateTime.Month == cursor.Month)
                .Select(e => e.Id)
                .ToHashSet();

            allMonths.Add(new MonthlyAttendanceTrendDto
            {
                Year = cursor.Year,
                Month = cursor.Month,
                TotalAttendees = attendance
                    .Count(a => eventIdsThisMonth.Contains(a.EventId)
                             && a.Status == AttendanceStatus.Present)
            });
            cursor = cursor.AddMonths(1);
        }

        var totalAttended = attendance.Count(a => a.Status == AttendanceStatus.Present);

        return ApiResponse<AttendanceReportDto>.SuccessResult(new AttendanceReportDto
        {
            From = request.From,
            To = request.To,
            TotalEventSessions = events.Count,
            TotalAttendanceRecords = totalAttended,
            AverageAttendancePerEvent = events.Count > 0
                ? Math.Round((double)totalAttended / events.Count, 1) : 0,
            ByEvent = byEvent,
            MonthlyTrend = allMonths
        });
    }
}
