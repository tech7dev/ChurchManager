using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Sunday school class (e.g. Youth, Elementary, Adults).
/// </summary>
public class SundaySchoolClass : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ClassLevel Level { get; set; }
    public bool IsActive { get; set; } = true;

    // Teacher (member)
    public Guid? TeacherId { get; set; }
    public Member? Teacher { get; set; }

    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public int? MaxCapacity { get; set; }

    public ICollection<SundaySchoolLesson> Lessons { get; set; } = new List<SundaySchoolLesson>();
    public ICollection<SundaySchoolEnrollment> Enrollments { get; set; } = new List<SundaySchoolEnrollment>();
}
