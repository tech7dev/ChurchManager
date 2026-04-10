using SQLite;

namespace ChurchMS.MAUI.Models;

[Table("Attendance")]
public class LocalAttendance
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public string? VisitorName { get; set; }
    public DateOnly AttendanceDate { get; set; }
    public string Status { get; set; } = "Present";
    public bool RecordedByQr { get; set; }
    public bool IsSynced { get; set; }
    public DateTime RecordedAt { get; set; }
}
