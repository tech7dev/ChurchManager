using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// SMS credit wallet for a church — tracks available balance and lifetime totals.
/// One record per church.
/// </summary>
public class SmsCredit : TenantEntity
{
    public int Balance { get; set; }
    public int TotalPurchased { get; set; }
    public int TotalConsumed { get; set; }

    public ICollection<SmsCreditTransaction> Transactions { get; set; } = [];
}
