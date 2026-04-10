using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Accounting.DTOs;

public class BankAccountDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string? BranchName { get; set; }
    public string? SwiftCode { get; set; }
    public string? RoutingNumber { get; set; }
    public BankAccountType AccountType { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
}
