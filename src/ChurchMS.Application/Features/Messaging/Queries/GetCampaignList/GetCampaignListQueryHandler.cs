using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Queries.GetCampaignList;

public class GetCampaignListQueryHandler(
    IRepository<MessageCampaign> campaignRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetCampaignListQuery, ApiResponse<PagedResult<MessageCampaignDto>>>
{
    public async Task<ApiResponse<PagedResult<MessageCampaignDto>>> Handle(
        GetCampaignListQuery request, CancellationToken cancellationToken)
    {
        var all = await campaignRepository.FindAsync(
            c => (!request.Channel.HasValue || c.Channel == request.Channel.Value)
              && (!request.Status.HasValue || c.Status == request.Status.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(c => c.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<MessageCampaignDto>();
        foreach (var c in paged)
        {
            string? senderName = null;
            if (c.SentByMemberId.HasValue)
            {
                var sender = await memberRepository.GetByIdAsync(c.SentByMemberId.Value, cancellationToken);
                senderName = sender is not null ? $"{sender.FirstName} {sender.LastName}" : null;
            }

            dtos.Add(new MessageCampaignDto
            {
                Id = c.Id,
                ChurchId = c.ChurchId,
                Title = c.Title,
                Body = c.Body,
                Channel = c.Channel,
                Status = c.Status,
                ScheduledAt = c.ScheduledAt,
                SentAt = c.SentAt,
                SentByMemberId = c.SentByMemberId,
                SentByName = senderName,
                RecipientCount = c.RecipientCount,
                DeliveredCount = c.DeliveredCount,
                FailedCount = c.FailedCount,
                CreatedAt = c.CreatedAt
            });
        }

        return ApiResponse<PagedResult<MessageCampaignDto>>.SuccessResult(new PagedResult<MessageCampaignDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
