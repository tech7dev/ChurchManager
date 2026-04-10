using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Members.DTOs;

public class VisitorDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Gender? Gender { get; set; }
    public DateOnly? FirstVisitDate { get; set; }
    public string? HowHeardAboutUs { get; set; }
    public string? Notes { get; set; }
    public VisitorStatus Status { get; set; }
    public Guid? ConvertedToMemberId { get; set; }
    public DateTime? ConvertedAt { get; set; }
    public Guid? AssignedToMemberId { get; set; }
    public string? AssignedToMemberName { get; set; }
    public DateTime CreatedAt { get; set; }
}
