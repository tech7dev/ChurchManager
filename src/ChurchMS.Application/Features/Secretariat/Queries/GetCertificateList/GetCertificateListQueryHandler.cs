using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Queries.GetCertificateList;

public class GetCertificateListQueryHandler(
    IRepository<Certificate> certificateRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetCertificateListQuery, ApiResponse<PagedResult<CertificateDto>>>
{
    public async Task<ApiResponse<PagedResult<CertificateDto>>> Handle(
        GetCertificateListQuery request, CancellationToken cancellationToken)
    {
        var all = await certificateRepository.FindAsync(
            c => (!request.Type.HasValue || c.Type == request.Type.Value)
              && (!request.MemberId.HasValue || c.MemberId == request.MemberId.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(c => c.IssuedDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<CertificateDto>();
        foreach (var c in paged)
        {
            var member = await memberRepository.GetByIdAsync(c.MemberId, cancellationToken);

            string? issuerName = null;
            if (c.IssuedByMemberId.HasValue)
            {
                var issuer = await memberRepository.GetByIdAsync(c.IssuedByMemberId.Value, cancellationToken);
                issuerName = issuer is not null ? $"{issuer.FirstName} {issuer.LastName}" : null;
            }

            dtos.Add(new CertificateDto
            {
                Id = c.Id,
                ChurchId = c.ChurchId,
                Type = c.Type,
                CertificateNumber = c.CertificateNumber,
                MemberId = c.MemberId,
                MemberName = member is not null ? $"{member.FirstName} {member.LastName}" : "",
                IssuedDate = c.IssuedDate,
                IssuedByMemberId = c.IssuedByMemberId,
                IssuedByName = issuerName,
                FileUrl = c.FileUrl,
                Notes = c.Notes,
                CreatedAt = c.CreatedAt
            });
        }

        return ApiResponse<PagedResult<CertificateDto>>.SuccessResult(new PagedResult<CertificateDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
