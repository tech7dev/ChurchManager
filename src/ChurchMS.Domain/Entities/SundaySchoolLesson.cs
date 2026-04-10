using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A lesson/session within a Sunday school class.
/// </summary>
public class SundaySchoolLesson : TenantEntity
{
    public Guid ClassId { get; set; }
    public SundaySchoolClass Class { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LessonNotes { get; set; }
    public DateOnly LessonDate { get; set; }
    public int? DurationMinutes { get; set; }

    public ICollection<SundaySchoolAttendance> AttendanceRecords { get; set; } = new List<SundaySchoolAttendance>();
}
