using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Designated giving fund (e.g. Tithe, Offering, Building Fund, Missions).
/// </summary>
public class Fund : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
}
