using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Individual credit purchase or consumption event.
/// </summary>
public class SmsCreditTransaction : TenantEntity
{
    public Guid SmsCreditId { get; set; }
    public SmsCredit SmsCredit { get; set; } = null!;
    public SmsCreditTransactionType Type { get; set; }

    /// <summary>Positive for purchases/refunds, negative for consumption.</summary>
    public int Amount { get; set; }

    public int BalanceBefore { get; set; }
    public int BalanceAfter { get; set; }

    /// <summary>Payment transaction ID or message campaign ID.</summary>
    public string? Reference { get; set; }
    public string? Notes { get; set; }
}
