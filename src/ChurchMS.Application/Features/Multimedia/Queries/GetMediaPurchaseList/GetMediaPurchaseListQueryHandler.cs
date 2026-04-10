using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Queries.GetMediaPurchaseList;

public class GetMediaPurchaseListQueryHandler(
    IRepository<MediaPurchase> purchaseRepository,
    IRepository<MediaContent> contentRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetMediaPurchaseListQuery, ApiResponse<PagedResult<MediaPurchaseDto>>>
{
    public async Task<ApiResponse<PagedResult<MediaPurchaseDto>>> Handle(
        GetMediaPurchaseListQuery request, CancellationToken cancellationToken)
    {
        var all = await purchaseRepository.FindAsync(
            p => (!request.ContentId.HasValue || p.ContentId == request.ContentId.Value)
              && (!request.MemberId.HasValue || p.MemberId == request.MemberId.Value)
              && (!request.Status.HasValue || p.Status == request.Status.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<MediaPurchaseDto>();
        foreach (var purchase in paged)
        {
            var content = await contentRepository.GetByIdAsync(purchase.ContentId, cancellationToken);
            var member = await memberRepository.GetByIdAsync(purchase.MemberId, cancellationToken);

            string? activatedByName = null;
            if (purchase.ActivatedByMemberId.HasValue)
            {
                var activatedBy = await memberRepository.GetByIdAsync(purchase.ActivatedByMemberId.Value, cancellationToken);
                activatedByName = activatedBy is not null ? $"{activatedBy.FirstName} {activatedBy.LastName}" : null;
            }

            dtos.Add(new MediaPurchaseDto
            {
                Id = purchase.Id,
                ChurchId = purchase.ChurchId,
                ContentId = purchase.ContentId,
                ContentTitle = content?.Title ?? "",
                MemberId = purchase.MemberId,
                MemberName = member is not null ? $"{member.FirstName} {member.LastName}" : "",
                Amount = purchase.Amount,
                Status = purchase.Status,
                PaymentReference = purchase.PaymentReference,
                PaidAt = purchase.PaidAt,
                ActivatedAt = purchase.ActivatedAt,
                ActivatedByMemberId = purchase.ActivatedByMemberId,
                ActivatedByName = activatedByName,
                CreatedAt = purchase.CreatedAt
            });
        }

        return ApiResponse<PagedResult<MediaPurchaseDto>>.SuccessResult(new PagedResult<MediaPurchaseDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
