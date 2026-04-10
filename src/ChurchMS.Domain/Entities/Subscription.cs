using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Tracks a church's subscription to a platform plan, including billing and payment details.
/// </summary>
public class Subscription : TenantEntity
{
    public SubscriptionPlan Plan { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Trial;
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public bool AutoRenew { get; set; } = true;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? TrialEndDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public DateTime? CancellationDate { get; set; }
    public string? CancellationReason { get; set; }

    public PaymentMethod? PaymentMethod { get; set; }

    /// <summary>External subscription ID from Stripe, PayPal, etc.</summary>
    public string? ExternalSubscriptionId { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = [];
}
