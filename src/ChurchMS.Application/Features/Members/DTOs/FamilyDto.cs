namespace ChurchMS.Application.Features.Members.DTOs;

public class FamilyDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int MemberCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
