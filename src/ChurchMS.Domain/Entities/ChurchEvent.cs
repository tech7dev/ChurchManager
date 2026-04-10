using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A church event: service, conference, retreat, concert, workshop, etc.
/// </summary>
public class ChurchEvent : TenantEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public EventType Type { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Draft;

    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Location { get; set; }
    public string? OnlineLink { get; set; }

    public bool RequiresRegistration { get; set; }
    public int? MaxAttendees { get; set; }
    public decimal? RegistrationFee { get; set; }
    public string? Currency { get; set; }

    // QR code for event sharing / check-in
    public string? QrCodeValue { get; set; }

    // Recurrence
    public bool IsRecurring { get; set; }
    public RecurrenceFrequency? RecurrenceFrequency { get; set; }
    public DateOnly? RecurrenceEndDate { get; set; }

    public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
    public ICollection<EventAttendance> AttendanceRecords { get; set; } = new List<EventAttendance>();
}
