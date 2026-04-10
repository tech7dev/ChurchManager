using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// An audience/appointment request from a member to a church responsible.
/// Workflow: Pending → Scheduled → Confirmed → Completed / Cancelled.
/// </summary>
public class Appointment : TenantEntity
{
    /// <summary>Member who requested the appointment.</summary>
    public Guid MemberId { get; set; }

    /// <summary>Staff member or pastor responsible for the meeting.</summary>
    public Guid ResponsibleMemberId { get; set; }

    public string Subject { get; set; } = null!;
    public string? Description { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public MeetingType MeetingType { get; set; } = MeetingType.InPerson;

    public DateTime RequestedAt { get; set; }

    /// <summary>Confirmed date/time for the meeting.</summary>
    public DateTime? ScheduledAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    /// <summary>Link for video-call meetings.</summary>
    public string? VideoCallLink { get; set; }
    public string? Location { get; set; }

    public bool ReminderSent10Min { get; set; }
    public bool ReminderSent5Min { get; set; }

    public string? Notes { get; set; }

    // Navigation
    public Member Member { get; set; } = null!;
    public Member Responsible { get; set; } = null!;
}
