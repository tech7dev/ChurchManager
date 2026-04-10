namespace ChurchMS.Application.Features.GrowthSchool.DTOs;

public class GrowthSchoolSessionDto
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SessionNotes { get; set; }
    public DateOnly SessionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public int AttendanceCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
