using ChurchMS.Domain.Entities;

namespace ChurchMS.Domain.Common;

/// <summary>
/// Base class for all church-scoped (tenant) entities.
/// </summary>
public abstract class TenantEntity : BaseEntity
{
    public Guid ChurchId { get; set; }
    public Church Church { get; set; } = null!;
}
