using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Expenses.DTOs;

public class ExpenseListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly ExpenseDate { get; set; }
    public ExpenseStatus Status { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? VendorName { get; set; }
}
