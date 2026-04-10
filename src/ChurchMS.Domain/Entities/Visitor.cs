using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Church visitor who has not yet become a member.
/// </summary>
public class Visitor : TenantEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Gender? Gender { get; set; }
    public DateOnly? FirstVisitDate { get; set; }
    public string? HowHeardAboutUs { get; set; }
    public string? Notes { get; set; }
    public VisitorStatus Status { get; set; } = VisitorStatus.New;

    // If visitor was converted to a member
    public Guid? ConvertedToMemberId { get; set; }
    public Member? ConvertedToMember { get; set; }
    public DateTime? ConvertedAt { get; set; }

    // Who is following up
    public Guid? AssignedToMemberId { get; set; }
    public Member? AssignedToMember { get; set; }
}
