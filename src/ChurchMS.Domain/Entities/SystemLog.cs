using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Audit log entry for significant system events within a church.
/// </summary>
public class SystemLog : TenantEntity
{
    public string Action { get; set; } = string.Empty;
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public string? Details { get; set; }
    public Guid? UserId { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    /// <summary>Severity level: Info, Warning, Error.</summary>
    public string Level { get; set; } = "Info";
}
