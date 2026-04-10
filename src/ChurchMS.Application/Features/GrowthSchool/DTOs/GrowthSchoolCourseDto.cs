using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.GrowthSchool.DTOs;

public class GrowthSchoolCourseDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public GrowthSchoolLevel Level { get; set; }
    public bool IsActive { get; set; }
    public Guid? InstructorId { get; set; }
    public string? InstructorName { get; set; }
    public int? DurationWeeks { get; set; }
    public int? MaxCapacity { get; set; }
    public int EnrollmentCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
