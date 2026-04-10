using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Commands.CreateMessageCampaign;

public record CreateMessageCampaignCommand(
    string Title,
    string Body,
    MessageChannel Channel,
    DateTime? ScheduledAt
) : IRequest<ApiResponse<MessageCampaignDto>>;
