using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Subscriptions.DTOs;

public class SubscriptionDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public SubscriptionPlan Plan { get; set; }
    public SubscriptionStatus Status { get; set; }
    public BillingCycle BillingCycle { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public bool AutoRenew { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? TrialEndDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public DateTime? CancellationDate { get; set; }
    public string? CancellationReason { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public string? ExternalSubscriptionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive => Status == SubscriptionStatus.Active || Status == SubscriptionStatus.Trial;
}

public class InvoiceDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string InvoiceNumber { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public InvoiceStatus Status { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidAt { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public string? PaymentReference { get; set; }
    public string? Notes { get; set; }
    public Guid? SubscriptionId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SmsCreditDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public int Balance { get; set; }
    public int TotalPurchased { get; set; }
    public int TotalConsumed { get; set; }
}

public class SmsCreditTransactionDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public SmsCreditTransactionType Type { get; set; }
    public int Amount { get; set; }
    public int BalanceBefore { get; set; }
    public int BalanceAfter { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
