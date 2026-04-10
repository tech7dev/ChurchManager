using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Departments.DTOs;

public class DepartmentMemberDto
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = null!;
    public DepartmentMemberRole Role { get; set; }
    public DateOnly JoinedDate { get; set; }
    public DateOnly? LeftDate { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
