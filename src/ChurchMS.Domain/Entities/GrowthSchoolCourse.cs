using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A discipleship / growth school course (e.g. "New Believers", "Leadership 101").
/// </summary>
public class GrowthSchoolCourse : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public GrowthSchoolLevel Level { get; set; }
    public bool IsActive { get; set; } = true;

    /// <summary>Instructor (a church member).</summary>
    public Guid? InstructorId { get; set; }
    public Member? Instructor { get; set; }

    public int? DurationWeeks { get; set; }
    public int? MaxCapacity { get; set; }

    public ICollection<GrowthSchoolSession> Sessions { get; set; } = new List<GrowthSchoolSession>();
    public ICollection<GrowthSchoolEnrollment> Enrollments { get; set; } = new List<GrowthSchoolEnrollment>();
}
