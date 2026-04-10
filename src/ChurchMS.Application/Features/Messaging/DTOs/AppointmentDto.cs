using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Messaging.DTOs;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = null!;
    public Guid ResponsibleMemberId { get; set; }
    public string ResponsibleName { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string? Description { get; set; }
    public AppointmentStatus Status { get; set; }
    public MeetingType MeetingType { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? VideoCallLink { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
