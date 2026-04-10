using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.SundaySchool.DTOs;

public class SundaySchoolClassDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ClassLevel Level { get; set; }
    public bool IsActive { get; set; }
    public Guid? TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public int? MaxCapacity { get; set; }
    public int EnrollmentCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
