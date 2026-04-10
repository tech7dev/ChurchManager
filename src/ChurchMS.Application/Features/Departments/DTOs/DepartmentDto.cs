namespace ChurchMS.Application.Features.Departments.DTOs;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid? LeaderId { get; set; }
    public string? LeaderName { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; }
    public int MemberCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
