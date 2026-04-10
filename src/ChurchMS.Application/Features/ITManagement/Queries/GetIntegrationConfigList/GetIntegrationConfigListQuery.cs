using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetIntegrationConfigList;

public record GetIntegrationConfigListQuery(
    IntegrationService? Service = null
) : IRequest<ApiResponse<List<IntegrationConfigDto>>>;
