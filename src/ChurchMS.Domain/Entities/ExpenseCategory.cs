using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Category for grouping expenses (e.g. Utilities, Salaries, Maintenance).
/// </summary>
public class ExpenseCategory : TenantEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Color { get; set; } // hex for UI display
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
