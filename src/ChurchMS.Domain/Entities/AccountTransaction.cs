using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Ledger transaction against a bank account.
/// </summary>
public class AccountTransaction : TenantEntity
{
    public Guid BankAccountId { get; set; }
    public BankAccount BankAccount { get; set; } = null!;
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public decimal RunningBalance { get; set; }

    // Links to source documents
    public Guid? ContributionId { get; set; }
    public Guid? ExpenseId { get; set; }
}
