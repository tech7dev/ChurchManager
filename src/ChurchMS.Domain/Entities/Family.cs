using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Family group linking related members within a church.
/// </summary>
public class Family : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public ICollection<Member> Members { get; set; } = new List<Member>();
}
