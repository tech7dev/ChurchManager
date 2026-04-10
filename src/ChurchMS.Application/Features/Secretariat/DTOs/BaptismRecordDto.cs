namespace ChurchMS.Application.Features.Secretariat.DTOs;

public class BaptismRecordDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = null!;
    public DateOnly BaptismDate { get; set; }
    public Guid? OfficiantMemberId { get; set; }
    public string? OfficiantName { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public Guid? CertificateId { get; set; }
    public string? CertificateNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}
