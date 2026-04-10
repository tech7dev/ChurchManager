using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Attendance record for a Sunday school lesson.
/// </summary>
public class SundaySchoolAttendance : TenantEntity
{
    public Guid LessonId { get; set; }
    public SundaySchoolLesson Lesson { get; set; } = null!;

    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
    public string? Notes { get; set; }
}
