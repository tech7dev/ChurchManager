using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Expenses.DTOs;

public class BudgetDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal Remaining => TotalAmount - TotalSpent;
    public BudgetStatus Status { get; set; }
    public string? Notes { get; set; }
    public IList<BudgetLineDto> Lines { get; set; } = new List<BudgetLineDto>();
    public DateTime CreatedAt { get; set; }
}

public class BudgetLineDto
{
    public Guid Id { get; set; }
    public Guid BudgetId { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal AllocatedAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal Remaining => AllocatedAmount - SpentAmount;
    public string? Notes { get; set; }
}
