using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A support ticket submitted by church staff to the platform support team.
/// </summary>
public class SupportTicket : TenantEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketCategory Category { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public Guid SubmittedByUserId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string? ResolutionNotes { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public ICollection<SupportTicketComment> Comments { get; set; } = [];
}
