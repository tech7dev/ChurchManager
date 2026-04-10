using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Evangelism.DTOs;

public class EvangelismFollowUpDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public Guid ContactId { get; set; }
    public FollowUpMethod Method { get; set; }
    public DateOnly FollowUpDate { get; set; }
    public string? Notes { get; set; }
    public Guid? ConductedByMemberId { get; set; }
    public string? ConductedByName { get; set; }
    public DateTime CreatedAt { get; set; }
}
