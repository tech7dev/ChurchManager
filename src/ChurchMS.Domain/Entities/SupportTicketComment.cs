using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A comment on a support ticket, authored by staff or platform support.
/// </summary>
public class SupportTicketComment : TenantEntity
{
    public Guid TicketId { get; set; }
    public SupportTicket Ticket { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;

    /// <summary>Internal notes visible only to platform staff.</summary>
    public bool IsInternal { get; set; }
}
