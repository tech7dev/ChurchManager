using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.SundaySchool.DTOs;

public class EnrollmentDto
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public DateOnly EnrolledDate { get; set; }
    public DateOnly? GraduatedDate { get; set; }
    public EnrollmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
