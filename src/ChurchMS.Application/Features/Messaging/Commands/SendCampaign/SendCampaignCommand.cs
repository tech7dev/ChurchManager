using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Commands.SendCampaign;

/// <summary>Immediately marks a Draft/Scheduled campaign as Sending and queues delivery.</summary>
public record SendCampaignCommand(Guid CampaignId) : IRequest<ApiResponse<MessageCampaignDto>>;
