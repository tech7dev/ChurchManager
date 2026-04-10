using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.GrowthSchool.DTOs;

public class GrowthEnrollmentDto
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public DateOnly EnrolledDate { get; set; }
    public DateOnly? CompletedDate { get; set; }
    public EnrollmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
