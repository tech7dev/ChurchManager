using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Attendance record for a recurring event occurrence (e.g. Sunday service).
/// </summary>
public class EventAttendance : TenantEntity
{
    public Guid EventId { get; set; }
    public ChurchEvent Event { get; set; } = null!;

    public Guid? MemberId { get; set; }
    public Member? Member { get; set; }
    public string? VisitorName { get; set; }

    public DateOnly AttendanceDate { get; set; }
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
    public string? Notes { get; set; }

    // How attendance was recorded
    public bool RecordedByQr { get; set; }
    public Guid? RecordedByMemberId { get; set; }
}
