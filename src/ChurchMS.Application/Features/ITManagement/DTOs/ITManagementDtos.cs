using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.ITManagement.DTOs;

public class SupportTicketDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TicketCategory Category { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public Guid SubmittedByUserId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public string? ResolutionNotes { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<SupportTicketCommentDto> Comments { get; set; } = [];
}

public class SupportTicketCommentDto
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = null!;
    public bool IsInternal { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class IntegrationConfigDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public IntegrationService Service { get; set; }
    public bool IsEnabled { get; set; }
    public string? WebhookUrl { get; set; }
    public string? AdditionalConfig { get; set; }
    public DateTime? LastTestedAt { get; set; }
    public bool? IsHealthy { get; set; }
    public string? LastTestResult { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    // Note: ApiKey and ApiSecret are intentionally excluded for security
}

public class SystemLogDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Action { get; set; } = null!;
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public string? Details { get; set; }
    public Guid? UserId { get; set; }
    public string? IpAddress { get; set; }
    public string Level { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public class UserSummaryDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid? ChurchId { get; set; }
    public bool IsActive { get; set; }
    public IList<string> Roles { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
