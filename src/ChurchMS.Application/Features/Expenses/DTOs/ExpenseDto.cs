using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Expenses.DTOs;

public class ExpenseDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly ExpenseDate { get; set; }
    public ExpenseStatus Status { get; set; }
    public ContributionType PaymentMethod { get; set; }
    public string? ReceiptUrl { get; set; }
    public string? VendorName { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid? BankAccountId { get; set; }
    public string? BankAccountName { get; set; }
    public Guid? SubmittedByMemberId { get; set; }
    public Guid? ApprovedByMemberId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }
    public Guid? BudgetLineId { get; set; }
    public DateTime CreatedAt { get; set; }
}
