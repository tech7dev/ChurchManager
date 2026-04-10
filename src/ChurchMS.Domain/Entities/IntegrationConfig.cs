using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Configuration for an external service integration for a specific church.
/// </summary>
public class IntegrationConfig : TenantEntity
{
    public IntegrationService Service { get; set; }
    public bool IsEnabled { get; set; }
    public string? ApiKey { get; set; }
    public string? ApiSecret { get; set; }
    public string? WebhookUrl { get; set; }

    /// <summary>JSON string for any additional service-specific settings.</summary>
    public string? AdditionalConfig { get; set; }

    public DateTime? LastTestedAt { get; set; }
    public bool? IsHealthy { get; set; }
    public string? LastTestResult { get; set; }
}
