namespace ChurchMS.Application.Features.SundaySchool.DTOs;

public class SundaySchoolLessonDto
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LessonNotes { get; set; }
    public DateOnly LessonDate { get; set; }
    public int? DurationMinutes { get; set; }
    public int AttendanceCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
