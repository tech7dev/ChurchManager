using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Commands.RegisterDocument;

public class RegisterDocumentCommandHandler(
    IRepository<Document> documentRepository,
    IRepository<Member> memberRepository,
    ICurrentUserService currentUserService,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterDocumentCommand, ApiResponse<DocumentDto>>
{
    public async Task<ApiResponse<DocumentDto>> Handle(
        RegisterDocumentCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()!.Value;

        var document = new Document
        {
            ChurchId = churchId,
            Title = request.Title,
            FileName = request.FileName,
            FileUrl = request.FileUrl,
            FileSize = request.FileSize,
            ContentType = request.ContentType,
            Type = request.Type,
            MemberId = request.MemberId,
            UploadedByMemberId = currentUserService.GetUserId(),
            Notes = request.Notes
        };

        await documentRepository.AddAsync(document, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? memberName = null;
        if (request.MemberId.HasValue)
        {
            var member = await memberRepository.GetByIdAsync(request.MemberId.Value, cancellationToken);
            memberName = member is not null ? $"{member.FirstName} {member.LastName}" : null;
        }

        string? uploaderName = null;
        if (document.UploadedByMemberId.HasValue)
        {
            var uploader = await memberRepository.GetByIdAsync(document.UploadedByMemberId.Value, cancellationToken);
            uploaderName = uploader is not null ? $"{uploader.FirstName} {uploader.LastName}" : null;
        }

        return ApiResponse<DocumentDto>.SuccessResult(new DocumentDto
        {
            Id = document.Id,
            ChurchId = document.ChurchId,
            Title = document.Title,
            FileName = document.FileName,
            FileUrl = document.FileUrl,
            FileSize = document.FileSize,
            ContentType = document.ContentType,
            Type = document.Type,
            MemberId = document.MemberId,
            MemberName = memberName,
            UploadedByMemberId = document.UploadedByMemberId,
            UploadedByName = uploaderName,
            Notes = document.Notes,
            CreatedAt = document.CreatedAt
        });
    }
}
