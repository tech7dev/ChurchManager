using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Secretariat.DTOs;

public class CertificateDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public CertificateType Type { get; set; }
    public string CertificateNumber { get; set; } = null!;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = null!;
    public DateOnly IssuedDate { get; set; }
    public Guid? IssuedByMemberId { get; set; }
    public string? IssuedByName { get; set; }
    public string? FileUrl { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
