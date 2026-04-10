namespace ChurchMS.Application.Features.Secretariat.DTOs;

public class MarriageRecordDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public Guid Spouse1MemberId { get; set; }
    public string Spouse1Name { get; set; } = null!;
    public Guid? Spouse2MemberId { get; set; }
    public string? Spouse2Name { get; set; }
    public string? Spouse2Phone { get; set; }
    public DateOnly MarriageDate { get; set; }
    public Guid? OfficiantMemberId { get; set; }
    public string? OfficiantName { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public Guid? CertificateId { get; set; }
    public string? CertificateNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}
