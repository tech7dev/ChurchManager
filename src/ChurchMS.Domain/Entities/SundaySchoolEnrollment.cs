using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A member's enrollment in a Sunday school class.
/// </summary>
public class SundaySchoolEnrollment : TenantEntity
{
    public Guid ClassId { get; set; }
    public SundaySchoolClass Class { get; set; } = null!;

    public Guid MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public DateOnly EnrolledDate { get; set; }
    public DateOnly? GraduatedDate { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    public string? Notes { get; set; }
}
