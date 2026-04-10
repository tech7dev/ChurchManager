using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Commands.RegisterDocument;

public record RegisterDocumentCommand(
    string Title,
    string FileName,
    string FileUrl,
    long FileSize,
    string? ContentType,
    DocumentType Type,
    Guid? MemberId,
    string? Notes
) : IRequest<ApiResponse<DocumentDto>>;
