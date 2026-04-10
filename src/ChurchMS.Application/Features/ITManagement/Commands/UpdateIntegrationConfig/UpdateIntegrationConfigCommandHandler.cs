using ChurchMS.Application.Features.ITManagement.Commands.CreateIntegrationConfig;
using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.UpdateIntegrationConfig;

public class UpdateIntegrationConfigCommandHandler(
    IRepository<IntegrationConfig> configRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateIntegrationConfigCommand, ApiResponse<IntegrationConfigDto>>
{
    public async Task<ApiResponse<IntegrationConfigDto>> Handle(
        UpdateIntegrationConfigCommand request, CancellationToken cancellationToken)
    {
        var config = await configRepository.GetByIdAsync(request.ConfigId, cancellationToken);
        if (config is null)
            return ApiResponse<IntegrationConfigDto>.FailureResult("Integration config not found.");

        config.IsEnabled = request.IsEnabled;
        config.WebhookUrl = request.WebhookUrl;
        config.AdditionalConfig = request.AdditionalConfig;

        // Only update credentials if explicitly provided
        if (request.ApiKey is not null)
            config.ApiKey = request.ApiKey;
        if (request.ApiSecret is not null)
            config.ApiSecret = request.ApiSecret;

        // Reset health status on update so it gets re-tested
        config.IsHealthy = null;
        config.LastTestResult = null;

        configRepository.Update(config);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<IntegrationConfigDto>.SuccessResult(
            CreateIntegrationConfigCommandHandler.MapToDto(config));
    }
}
