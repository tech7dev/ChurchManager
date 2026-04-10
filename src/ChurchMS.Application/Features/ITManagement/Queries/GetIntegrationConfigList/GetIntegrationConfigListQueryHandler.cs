using ChurchMS.Application.Features.ITManagement.Commands.CreateIntegrationConfig;
using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetIntegrationConfigList;

public class GetIntegrationConfigListQueryHandler(IRepository<IntegrationConfig> configRepository)
    : IRequestHandler<GetIntegrationConfigListQuery, ApiResponse<List<IntegrationConfigDto>>>
{
    public async Task<ApiResponse<List<IntegrationConfigDto>>> Handle(
        GetIntegrationConfigListQuery request, CancellationToken cancellationToken)
    {
        var configs = await configRepository.FindAsync(
            c => !request.Service.HasValue || c.Service == request.Service.Value,
            cancellationToken);

        var dtos = configs
            .OrderBy(c => c.Service)
            .Select(CreateIntegrationConfigCommandHandler.MapToDto)
            .ToList();

        return ApiResponse<List<IntegrationConfigDto>>.SuccessResult(dtos);
    }
}
