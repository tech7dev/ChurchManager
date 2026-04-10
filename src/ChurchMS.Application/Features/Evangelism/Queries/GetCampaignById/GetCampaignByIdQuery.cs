using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Queries.GetCampaignById;

public record GetCampaignByIdQuery(Guid Id) : IRequest<ApiResponse<EvangelismCampaignDto>>;
