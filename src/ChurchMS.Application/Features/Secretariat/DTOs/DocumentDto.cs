using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Secretariat.DTOs;

public class DocumentDto
{
    public Guid Id { get; set; }
    public Guid ChurchId { get; set; }
    public string Title { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public long FileSize { get; set; }
    public string? ContentType { get; set; }
    public DocumentType Type { get; set; }
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public Guid? UploadedByMemberId { get; set; }
    public string? UploadedByName { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
