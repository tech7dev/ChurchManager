using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A line item within a budget, linked to an expense category.
/// </summary>
public class BudgetLine : TenantEntity
{
    public Guid BudgetId { get; set; }
    public Budget Budget { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public ExpenseCategory Category { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public decimal AllocatedAmount { get; set; }
    public decimal SpentAmount { get; set; } // updated via triggers/background
    public string? Notes { get; set; }
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
