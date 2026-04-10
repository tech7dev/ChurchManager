using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Attendance record for a growth school session.
/// </summary>
public class GrowthSchoolAttendance : TenantEntity
{
    public Guid SessionId { get; set; }
    public GrowthSchoolSession Session { get; set; } = null!;

    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
    public string? Notes { get; set; }
}
