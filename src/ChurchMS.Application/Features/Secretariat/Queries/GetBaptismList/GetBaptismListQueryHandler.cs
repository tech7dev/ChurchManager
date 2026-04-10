using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Queries.GetBaptismList;

public class GetBaptismListQueryHandler(
    IRepository<BaptismRecord> baptismRepository,
    IRepository<Certificate> certificateRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetBaptismListQuery, ApiResponse<PagedResult<BaptismRecordDto>>>
{
    public async Task<ApiResponse<PagedResult<BaptismRecordDto>>> Handle(
        GetBaptismListQuery request, CancellationToken cancellationToken)
    {
        var all = await baptismRepository.FindAsync(
            b => (!request.MemberId.HasValue || b.MemberId == request.MemberId.Value)
              && (!request.Year.HasValue || b.BaptismDate.Year == request.Year.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(b => b.BaptismDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<BaptismRecordDto>();
        foreach (var b in paged)
        {
            var member = await memberRepository.GetByIdAsync(b.MemberId, cancellationToken);

            string? officiantName = null;
            if (b.OfficiantMemberId.HasValue)
            {
                var officiant = await memberRepository.GetByIdAsync(b.OfficiantMemberId.Value, cancellationToken);
                officiantName = officiant is not null ? $"{officiant.FirstName} {officiant.LastName}" : null;
            }

            string? certNumber = null;
            if (b.CertificateId.HasValue)
            {
                var cert = await certificateRepository.GetByIdAsync(b.CertificateId.Value, cancellationToken);
                certNumber = cert?.CertificateNumber;
            }

            dtos.Add(new BaptismRecordDto
            {
                Id = b.Id,
                ChurchId = b.ChurchId,
                MemberId = b.MemberId,
                MemberName = member is not null ? $"{member.FirstName} {member.LastName}" : "",
                BaptismDate = b.BaptismDate,
                OfficiantMemberId = b.OfficiantMemberId,
                OfficiantName = officiantName,
                Location = b.Location,
                Notes = b.Notes,
                CertificateId = b.CertificateId,
                CertificateNumber = certNumber,
                CreatedAt = b.CreatedAt
            });
        }

        return ApiResponse<PagedResult<BaptismRecordDto>>.SuccessResult(new PagedResult<BaptismRecordDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
