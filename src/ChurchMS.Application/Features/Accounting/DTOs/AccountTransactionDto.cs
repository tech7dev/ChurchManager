using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Accounting.DTOs;

public class AccountTransactionDto
{
    public Guid Id { get; set; }
    public Guid BankAccountId { get; set; }
    public string BankAccountName { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly TransactionDate { get; set; }
    public string? Description { get; set; }
    public string? Reference { get; set; }
    public decimal RunningBalance { get; set; }
    public DateTime CreatedAt { get; set; }
}
