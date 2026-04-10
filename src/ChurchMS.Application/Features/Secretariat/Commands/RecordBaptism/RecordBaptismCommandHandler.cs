using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Commands.RecordBaptism;

public class RecordBaptismCommandHandler(
    IRepository<BaptismRecord> baptismRepository,
    IRepository<Certificate> certificateRepository,
    IRepository<Member> memberRepository,
    ICurrentUserService currentUserService,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RecordBaptismCommand, ApiResponse<BaptismRecordDto>>
{
    public async Task<ApiResponse<BaptismRecordDto>> Handle(
        RecordBaptismCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()!.Value;

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        Certificate? certificate = null;
        if (request.IssueCertificate)
        {
            var existingCount = await certificateRepository.CountAsync(
                c => c.ChurchId == churchId && c.Type == CertificateType.Baptism, cancellationToken);

            certificate = new Certificate
            {
                ChurchId = churchId,
                Type = CertificateType.Baptism,
                CertificateNumber = $"BAP-{request.BaptismDate.Year}-{existingCount + 1:D4}",
                MemberId = request.MemberId,
                IssuedDate = request.BaptismDate,
                IssuedByMemberId = currentUserService.GetUserId()
            };
            await certificateRepository.AddAsync(certificate, cancellationToken);
        }

        var record = new BaptismRecord
        {
            ChurchId = churchId,
            MemberId = request.MemberId,
            BaptismDate = request.BaptismDate,
            OfficiantMemberId = request.OfficiantMemberId,
            Location = request.Location,
            Notes = request.Notes,
            CertificateId = certificate?.Id
        };

        await baptismRepository.AddAsync(record, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? officiantName = null;
        if (request.OfficiantMemberId.HasValue)
        {
            var officiant = await memberRepository.GetByIdAsync(request.OfficiantMemberId.Value, cancellationToken);
            officiantName = officiant is not null ? $"{officiant.FirstName} {officiant.LastName}" : null;
        }

        return ApiResponse<BaptismRecordDto>.SuccessResult(new BaptismRecordDto
        {
            Id = record.Id,
            ChurchId = record.ChurchId,
            MemberId = record.MemberId,
            MemberName = $"{member.FirstName} {member.LastName}",
            BaptismDate = record.BaptismDate,
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
