using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Commands.CreateMessageCampaign;

public class CreateMessageCampaignCommandHandler(
    IRepository<MessageCampaign> campaignRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMessageCampaignCommand, ApiResponse<MessageCampaignDto>>
{
    public async Task<ApiResponse<MessageCampaignDto>> Handle(
        CreateMessageCampaignCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()!.Value;

        var campaign = new MessageCampaign
        {
            ChurchId = churchId,
            Title = request.Title,
            Body = request.Body,
            Channel = request.Channel,
            Status = request.ScheduledAt.HasValue
                ? MessageCampaignStatus.Scheduled
                : MessageCampaignStatus.Draft,
            ScheduledAt = request.ScheduledAt
        };

        await campaignRepository.AddAsync(campaign, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<MessageCampaignDto>.SuccessResult(new MessageCampaignDto
        {
            Id = campaign.Id,
            ChurchId = campaign.ChurchId,
            Title = campaign.Title,
            Body = campaign.Body,
            Channel = campaign.Channel,
            Status = campaign.Status,
            ScheduledAt = campaign.ScheduledAt,
            RecipientCount = 0,
            DeliveredCount = 0,
            FailedCount = 0,
            CreatedAt = campaign.CreatedAt
        });
    }
}
