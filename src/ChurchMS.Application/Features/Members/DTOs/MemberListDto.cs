using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Members.DTOs;

public class MemberListDto
{
    public Guid Id { get; set; }
    public string MembershipNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string? PhotoUrl { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Gender? Gender { get; set; }
    public MemberStatus Status { get; set; }
    public DateOnly? JoinDate { get; set; }
    public string? FamilyName { get; set; }
    public DateTime CreatedAt { get; set; }
}
