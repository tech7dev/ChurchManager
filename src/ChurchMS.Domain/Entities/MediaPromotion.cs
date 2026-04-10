using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A promotional discount applied to one or all media content items.
/// </summary>
public class MediaPromotion : TenantEntity
{
    /// <summary>Null means the promotion applies to all paid content.</summary>
    public Guid? ContentId { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    /// <summary>Promo code required to redeem (null = automatic).</summary>
    public string? Code { get; set; }

    /// <summary>Percentage discount (0–100). Mutually exclusive with DiscountAmount.</summary>
    public decimal? DiscountPercent { get; set; }

    /// <summary>Fixed amount discount. Mutually exclusive with DiscountPercent.</summary>
    public decimal? DiscountAmount { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public MediaContent? Content { get; set; }
}
