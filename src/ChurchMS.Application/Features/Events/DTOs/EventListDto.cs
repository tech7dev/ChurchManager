using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Events.DTOs;

public class EventListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public EventType Type { get; set; }
    public EventStatus Status { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Location { get; set; }
    public bool RequiresRegistration { get; set; }
    public int? MaxAttendees { get; set; }
    public int RegistrationCount { get; set; }
}
