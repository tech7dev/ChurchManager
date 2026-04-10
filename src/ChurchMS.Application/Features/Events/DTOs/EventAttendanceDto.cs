using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Events.DTOs;

public class EventAttendanceDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public string? VisitorName { get; set; }
    public DateOnly AttendanceDate { get; set; }
    public AttendanceStatus Status { get; set; }
    public bool RecordedByQr { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
