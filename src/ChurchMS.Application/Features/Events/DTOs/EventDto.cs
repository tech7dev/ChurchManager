using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Events.DTOs;

public class EventDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public EventType Type { get; set; }
    public EventStatus Status { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Location { get; set; }
    public string? OnlineLink { get; set; }
    public bool RequiresRegistration { get; set; }
    public int? MaxAttendees { get; set; }
    public decimal? RegistrationFee { get; set; }
    public string? Currency { get; set; }
    public string? QrCodeValue { get; set; }
    public bool IsRecurring { get; set; }
    public RecurrenceFrequency? RecurrenceFrequency { get; set; }
    public DateOnly? RecurrenceEndDate { get; set; }
    public int RegistrationCount { get; set; }
    public int AttendanceCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
