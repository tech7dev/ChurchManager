using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Events.DTOs;

public class EventRegistrationDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public string? GuestName { get; set; }
    public string? GuestEmail { get; set; }
    public string? GuestPhone { get; set; }
    public RegistrationStatus Status { get; set; }
    public string? RegistrationCode { get; set; }
    public decimal? AmountPaid { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? CheckedInAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
