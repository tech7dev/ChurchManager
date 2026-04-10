using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Queries.GetMarriageList;

public class GetMarriageListQueryHandler(
    IRepository<MarriageRecord> marriageRepository,
    IRepository<Certificate> certificateRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetMarriageListQuery, ApiResponse<PagedResult<MarriageRecordDto>>>
{
    public async Task<ApiResponse<PagedResult<MarriageRecordDto>>> Handle(
        GetMarriageListQuery request, CancellationToken cancellationToken)
    {
        var all = await marriageRepository.FindAsync(
            m => (!request.MemberId.HasValue ||
                  m.Spouse1MemberId == request.MemberId.Value ||
                  m.Spouse2MemberId == request.MemberId.Value)
              && (!request.Year.HasValue || m.MarriageDate.Year == request.Year.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(m => m.MarriageDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<MarriageRecordDto>();
        foreach (var m in paged)
        {
            var spouse1 = await memberRepository.GetByIdAsync(m.Spouse1MemberId, cancellationToken);

            Member? spouse2Member = null;
            if (m.Spouse2MemberId.HasValue)
                spouse2Member = await memberRepository.GetByIdAsync(m.Spouse2MemberId.Value, cancellationToken);

            string? officiantName = null;
            if (m.OfficiantMemberId.HasValue)
            {
                var officiant = await memberRepository.GetByIdAsync(m.OfficiantMemberId.Value, cancellationToken);
                officiantName = officiant is not null ? $"{officiant.FirstName} {officiant.LastName}" : null;
            }

            string? certNumber = null;
            if (m.CertificateId.HasValue)
            {
                var cert = await certificateRepository.GetByIdAsync(m.CertificateId.Value, cancellationToken);
                certNumber = cert?.CertificateNumber;
            }

            dtos.Add(new MarriageRecordDto
            {
                Id = m.Id,
                ChurchId = m.ChurchId,
                Spouse1MemberId = m.Spouse1MemberId,
                Spouse1Name = spouse1 is not null ? $"{spouse1.FirstName} {spouse1.LastName}" : "",
                Spouse2MemberId = m.Spouse2MemberId,
                Spouse2Name = spouse2Member is not null
                    ? $"{spouse2Member.FirstName} {spouse2Member.LastName}"
                    : m.Spouse2Name,
                Spouse2Phone = m.Spouse2Phone,
                MarriageDate = m.MarriageDate,
                OfficiantMemberId = m.OfficiantMemberId,
                OfficiantName = officiantName,
                Location = m.Location,
                Notes = m.Notes,
                CertificateId = m.CertificateId,
                CertificateNumber = certNumber,
                CreatedAt = m.CreatedAt
            });
        }

        return ApiResponse<PagedResult<MarriageRecordDto>>.SuccessResult(new PagedResult<MarriageRecordDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
