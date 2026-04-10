using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A ministry department within a church (e.g., Choir, Youth, Ushers).
/// </summary>
public class Department : TenantEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    /// <summary>FK to the Member who leads this department.</summary>
    public Guid? LeaderId { get; set; }

    /// <summary>Optional hex colour for UI display (#RRGGBB).</summary>
    public string? Color { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public Member? Leader { get; set; }
    public ICollection<DepartmentMember> Members { get; set; } = [];
    public ICollection<DepartmentTransaction> Transactions { get; set; } = [];
}
