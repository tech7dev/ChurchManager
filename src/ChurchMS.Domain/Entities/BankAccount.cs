using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Church bank account. Maximum 5 per church (business rule).
/// </summary>
public class BankAccount : TenantEntity
{
    public string AccountName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string? BranchName { get; set; }
    public string? SwiftCode { get; set; }
    public string? RoutingNumber { get; set; }
    public BankAccountType AccountType { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDefault { get; set; }
    public ICollection<AccountTransaction> Transactions { get; set; } = new List<AccountTransaction>();
}
