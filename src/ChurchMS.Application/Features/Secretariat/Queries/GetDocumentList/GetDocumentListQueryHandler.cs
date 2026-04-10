using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Queries.GetDocumentList;

public class GetDocumentListQueryHandler(
    IRepository<Document> documentRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetDocumentListQuery, ApiResponse<PagedResult<DocumentDto>>>
{
    public async Task<ApiResponse<PagedResult<DocumentDto>>> Handle(
        GetDocumentListQuery request, CancellationToken cancellationToken)
    {
        var all = await documentRepository.FindAsync(
            d => (!request.Type.HasValue || d.Type == request.Type.Value)
              && (!request.MemberId.HasValue || d.MemberId == request.MemberId.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(d => d.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<DocumentDto>();
        foreach (var d in paged)
        {
            string? memberName = null;
            if (d.MemberId.HasValue)
            {
                var member = await memberRepository.GetByIdAsync(d.MemberId.Value, cancellationToken);
                memberName = member is not null ? $"{member.FirstName} {member.LastName}" : null;
            }

            dtos.Add(new DocumentDto
            {
                Id = d.Id,
                ChurchId = d.ChurchId,
                Title = d.Title,
                FileName = d.FileName,
                FileUrl = d.FileUrl,
                FileSize = d.FileSize,
                ContentType = d.ContentType,
                Type = d.Type,
                MemberId = d.MemberId,
                MemberName = memberName,
                UploadedByMemberId = d.UploadedByMemberId,
                Notes = d.Notes,
                CreatedAt = d.CreatedAt
            });
        }

        return ApiResponse<PagedResult<DocumentDto>>.SuccessResult(new PagedResult<DocumentDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
