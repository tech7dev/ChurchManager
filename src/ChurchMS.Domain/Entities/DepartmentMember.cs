using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Links a church member to a department with an assigned role.
/// </summary>
public class DepartmentMember : TenantEntity
{
    public Guid DepartmentId { get; set; }
    public Guid MemberId { get; set; }
    public DepartmentMemberRole Role { get; set; } = DepartmentMemberRole.Member;
    public DateOnly JoinedDate { get; set; }
    public DateOnly? LeftDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }

    // Navigation
    public Department Department { get; set; } = null!;
    public Member Member { get; set; } = null!;
}
