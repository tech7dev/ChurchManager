using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Commands.IssueCertificate;

public class IssueCertificateCommandHandler(
    IRepository<Certificate> certificateRepository,
    IRepository<Member> memberRepository,
    ICurrentUserService currentUserService,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<IssueCertificateCommand, ApiResponse<CertificateDto>>
{
    public async Task<ApiResponse<CertificateDto>> Handle(
        IssueCertificateCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()!.Value;

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        // Generate a deterministic certificate number: {Type}-{Year}-{Seq}
        var existingCount = await certificateRepository.CountAsync(
            c => c.ChurchId == churchId && c.Type == request.Type, cancellationToken);

        var certNumber = $"{request.Type.ToString().ToUpper()[..3]}-{request.IssuedDate.Year}-{existingCount + 1:D4}";

        var certificate = new Certificate
        {
            ChurchId = churchId,
            Type = request.Type,
            CertificateNumber = certNumber,
            MemberId = request.MemberId,
            IssuedDate = request.IssuedDate,
            IssuedByMemberId = currentUserService.GetUserId(),
            Notes = request.Notes
        };

        await certificateRepository.AddAsync(certificate, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? issuerName = null;
        if (certificate.IssuedByMemberId.HasValue)
        {
            var issuer = await memberRepository.GetByIdAsync(certificate.IssuedByMemberId.Value, cancellationToken);
            issuerName = issuer is not null ? $"{issuer.FirstName} {issuer.LastName}" : null;
        }

        return ApiResponse<CertificateDto>.SuccessResult(new CertificateDto
        {
            Id = certificate.Id,
            ChurchId = certificate.ChurchId,
            Type = certificate.Type,
            CertificateNumber = certificate.CertificateNumber,
            MemberId = certificate.MemberId,
            MemberName = $"{member.FirstName} {member.LastName}",
            IssuedDate = certificate.IssuedDate,
            IssuedByMemberId = certificate.IssuedByMemberId,
            IssuedByName = issuerName,
            FileUrl = certificate.FileUrl,
            Notes = certificate.Notes,
            CreatedAt = certificate.CreatedAt
        });
    }
}
