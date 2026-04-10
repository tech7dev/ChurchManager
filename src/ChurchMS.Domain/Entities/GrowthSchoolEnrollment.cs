using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A member's enrollment in a growth school course.
/// </summary>
public class GrowthSchoolEnrollment : TenantEntity
{
    public Guid CourseId { get; set; }
    public GrowthSchoolCourse Course { get; set; } = null!;

    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public DateOnly EnrolledDate { get; set; }
    public DateOnly? CompletedDate { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    public string? Notes { get; set; }
}
