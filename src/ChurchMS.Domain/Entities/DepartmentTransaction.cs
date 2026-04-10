using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// An income or expense transaction recorded against a department.
/// </summary>
public class DepartmentTransaction : TenantEntity
{
    public Guid DepartmentId { get; set; }
    public DepartmentTransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
    public DateOnly Date { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid? RecordedByMemberId { get; set; }

    // Navigation
    public Department Department { get; set; } = null!;
    public Member? RecordedBy { get; set; }
}
