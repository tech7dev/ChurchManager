using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Church expense with approval workflow: Draft → Submitted → Approved/Rejected → Paid.
/// </summary>
public class Expense : TenantEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly ExpenseDate { get; set; }
    public ExpenseStatus Status { get; set; } = ExpenseStatus.Draft;
    public ContributionType PaymentMethod { get; set; }
    public string? ReceiptUrl { get; set; }
    public string? VendorName { get; set; }

    public Guid CategoryId { get; set; }
    public ExpenseCategory Category { get; set; } = null!;

    // Which bank account was used (set when Paid)
    public Guid? BankAccountId { get; set; }
    public BankAccount? BankAccount { get; set; }

    // Submitted by
    public Guid? SubmittedByMemberId { get; set; }

    // Approval
    public Guid? ApprovedByMemberId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }

    // Budget line association
    public Guid? BudgetLineId { get; set; }
    public BudgetLine? BudgetLine { get; set; }
}
