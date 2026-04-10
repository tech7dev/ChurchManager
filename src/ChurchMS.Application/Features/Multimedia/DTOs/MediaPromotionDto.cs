namespace ChurchMS.Application.Features.Multimedia.DTOs;

public class MediaPromotionDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public Guid? ContentId { get; set; }
    public string? ContentTitle { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
