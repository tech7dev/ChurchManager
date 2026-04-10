using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Commands.IssueCertificate;

public record IssueCertificateCommand(
    CertificateType Type,
    Guid MemberId,
    DateOnly IssuedDate,
    string? Notes
) : IRequest<ApiResponse<CertificateDto>>;
