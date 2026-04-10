using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Application.Exceptions;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Commands.SendCampaign;

public class SendCampaignCommandHandler(
    IRepository<MessageCampaign> campaignRepository,
    IRepository<Member> memberRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<SendCampaignCommand, ApiResponse<MessageCampaignDto>>
{
    public async Task<ApiResponse<MessageCampaignDto>> Handle(
        SendCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = await campaignRepository.GetByIdAsync(request.CampaignId, cancellationToken)
            ?? throw new NotFoundException(nameof(MessageCampaign), request.CampaignId);

        if (campaign.Status == MessageCampaignStatus.Sent)
            return ApiResponse<MessageCampaignDto>.FailureResult("Campaign has already been sent.");

        if (campaign.Status == MessageCampaignStatus.Cancelled)
            return ApiResponse<MessageCampaignDto>.FailureResult("Cannot send a cancelled campaign.");

        campaign.Status = MessageCampaignStatus.Sending;
        campaign.SentAt = DateTime.UtcNow;
        campaign.SentByMemberId = currentUserService.GetUserId();

        // In a real system, this would enqueue a background job via Hangfire.
        // For now we mark it Sent immediately.
        campaign.Status = MessageCampaignStatus.Sent;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? senderName = null;
        if (campaign.SentByMemberId.HasValue)
        {
            var sender = await memberRepository.GetByIdAsync(campaign.SentByMemberId.Value, cancellationToken);
            senderName = sender is not null ? $"{sender.FirstName} {sender.LastName}" : null;
        }

        return ApiResponse<MessageCampaignDto>.SuccessResult(new MessageCampaignDto
        {
            Id = campaign.Id,
            ChurchId = campaign.ChurchId,
            Title = campaign.Title,
            Body = campaign.Body,
            Channel = campaign.Channel,
            Status = campaign.Status,
            ScheduledAt = campaign.ScheduledAt,
            SentAt = campaign.SentAt,
            SentByMemberId = campaign.SentByMemberId,
            SentByName = senderName,
            RecipientCount = campaign.RecipientCount,
            DeliveredCount = campaign.DeliveredCount,
            FailedCount = campaign.FailedCount,
            CreatedAt = campaign.CreatedAt
        });
    }
}
