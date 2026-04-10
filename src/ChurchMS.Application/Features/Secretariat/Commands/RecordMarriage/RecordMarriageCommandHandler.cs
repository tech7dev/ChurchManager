using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Commands.RecordMarriage;

public class RecordMarriageCommandHandler(
    IRepository<MarriageRecord> marriageRepository,
    IRepository<Certificate> certificateRepository,
    IRepository<Member> memberRepository,
    ICurrentUserService currentUserService,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RecordMarriageCommand, ApiResponse<MarriageRecordDto>>
{
    public async Task<ApiResponse<MarriageRecordDto>> Handle(
        RecordMarriageCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()!.Value;

        var spouse1 = await memberRepository.GetByIdAsync(request.Spouse1MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.Spouse1MemberId);

        Member? spouse2Member = null;
        if (request.Spouse2MemberId.HasValue)
            spouse2Member = await memberRepository.GetByIdAsync(request.Spouse2MemberId.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(Member), request.Spouse2MemberId.Value);

        Certificate? certificate = null;
        if (request.IssueCertificate)
        {
            var existingCount = await certificateRepository.CountAsync(
                c => c.ChurchId == churchId && c.Type == CertificateType.Marriage, cancellationToken);

            certificate = new Certificate
            {
                ChurchId = churchId,
                Type = CertificateType.Marriage,
                CertificateNumber = $"MAR-{request.MarriageDate.Year}-{existingCount + 1:D4}",
                MemberId = request.Spouse1MemberId,
                IssuedDate = request.MarriageDate,
                IssuedByMemberId = currentUserService.GetUserId()
            };
            await certificateRepository.AddAsync(certificate, cancellationToken);
        }

        var record = new MarriageRecord
        {
            ChurchId = churchId,
            Spouse1MemberId = request.Spouse1MemberId,
            Spouse2MemberId = request.Spouse2MemberId,
            Spouse2Name = request.Spouse2Name,
            Spouse2Phone = request.Spouse2Phone,
            MarriageDate = request.MarriageDate,
            OfficiantMemberId = request.OfficiantMemberId,
            Location = request.Location,
            Notes = request.Notes,
            CertificateId = certificate?.Id
        };

        await marriageRepository.AddAsync(record, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? officiantName = null;
        if (request.OfficiantMemberId.HasValue)
        {
            var officiant = await memberRepository.GetByIdAsync(request.OfficiantMemberId.Value, cancellationToken);
            officiantName = officiant is not null ? $"{officiant.FirstName} {officiant.LastName}" : null;
        }

        return ApiResponse<MarriageRecordDto>.SuccessResult(new MarriageRecordDto
        {
            Id = record.Id,
            ChurchId = record.ChurchId,
            Spouse1MemberId = record.Spouse1MemberId,
            Spouse1Name = $"{spouse1.FirstName} {spouse1.LastName}",
            Spouse2MemberId = record.Spouse2MemberId,
            Spouse2Name = spouse2Member is not null
                ? $"{spouse2Member.FirstName} {spouse2Member.LastName}"
                : record.Spouse2Name,
            Spouse2Phone = record.Spouse2Phone,
            MarriageDate = record.MarriageDate,
            OfficiantMemberId = record.OfficiantMemberId,
            OfficiantName = officiantName,
            Location = record.Location,
            Notes = record.Notes,
            CertificateId = certificate?.Id,
            CertificateNumber = certificate?.CertificateNumber,
            CreatedAt = record.CreatedAt
        });
    }
}
