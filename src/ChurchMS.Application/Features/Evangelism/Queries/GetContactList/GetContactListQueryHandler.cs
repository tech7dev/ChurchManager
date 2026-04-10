using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Queries.GetContactList;

public class GetContactListQueryHandler(
    IRepository<EvangelismContact> contactRepository,
    IRepository<EvangelismFollowUp> followUpRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetContactListQuery, ApiResponse<PagedResult<EvangelismContactDto>>>
{
    public async Task<ApiResponse<PagedResult<EvangelismContactDto>>> Handle(
        GetContactListQuery request, CancellationToken cancellationToken)
    {
        var all = await contactRepository.FindAsync(
            c => (!request.CampaignId.HasValue || c.CampaignId == request.CampaignId.Value)
              && (!request.TeamId.HasValue || c.TeamId == request.TeamId.Value)
              && (!request.Status.HasValue || c.Status == request.Status.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(c => c.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<EvangelismContactDto>();
        foreach (var contact in paged)
        {
            string? assignedToName = null;
            if (contact.AssignedToMemberId.HasValue)
            {
                var assignedTo = await memberRepository.GetByIdAsync(contact.AssignedToMemberId.Value, cancellationToken);
                assignedToName = assignedTo is not null ? $"{assignedTo.FirstName} {assignedTo.LastName}" : null;
            }

            var followUpCount = await followUpRepository.CountAsync(f => f.ContactId == contact.Id, cancellationToken);

            dtos.Add(new EvangelismContactDto
            {
                Id = contact.Id,
                ChurchId = contact.ChurchId,
                CampaignId = contact.CampaignId,
                TeamId = contact.TeamId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
                Status = contact.Status,
                AssignedToMemberId = contact.AssignedToMemberId,
                AssignedToName = assignedToName,
                Notes = contact.Notes,
                ConvertedAt = contact.ConvertedAt,
                ConvertedMemberId = contact.ConvertedMemberId,
                FollowUpCount = followUpCount,
                CreatedAt = contact.CreatedAt
            });
        }

        return ApiResponse<PagedResult<EvangelismContactDto>>.SuccessResult(new PagedResult<EvangelismContactDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
