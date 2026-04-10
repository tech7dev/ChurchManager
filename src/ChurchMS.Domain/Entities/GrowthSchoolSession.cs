using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A single session / meeting within a growth school course.
/// </summary>
public class GrowthSchoolSession : TenantEntity
{
    public Guid CourseId { get; set; }
    public GrowthSchoolCourse Course { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SessionNotes { get; set; }
    public DateOnly SessionDate { get; set; }
    public int? DurationMinutes { get; set; }

    public ICollection<GrowthSchoolAttendance> AttendanceRecords { get; set; } = new List<GrowthSchoolAttendance>();
}
