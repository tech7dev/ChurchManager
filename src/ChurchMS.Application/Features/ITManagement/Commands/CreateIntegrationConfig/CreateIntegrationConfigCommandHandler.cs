using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.CreateIntegrationConfig;

public class CreateIntegrationConfigCommandHandler(
    IRepository<IntegrationConfig> configRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateIntegrationConfigCommand, ApiResponse<IntegrationConfigDto>>
{
    public async Task<ApiResponse<IntegrationConfigDto>> Handle(
        CreateIntegrationConfigCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<IntegrationConfigDto>.FailureResult("Church context required.");

        // Enforce one config per service per church
        var existing = await configRepository.FindAsync(
            c => c.ChurchId == churchId.Value && c.Service == request.Service,
            cancellationToken);

        if (existing.Count > 0)
            return ApiResponse<IntegrationConfigDto>.FailureResult(
                $"Integration config for {request.Service} already exists. Use update instead.");

        var config = new IntegrationConfig
        {
            ChurchId = churchId.Value,
            Service = request.Service,
            IsEnabled = request.IsEnabled,
            ApiKey = request.ApiKey,
            ApiSecret = request.ApiSecret,
            WebhookUrl = request.WebhookUrl,
            AdditionalConfig = request.AdditionalConfig
        };

        await configRepository.AddAsync(config, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<IntegrationConfigDto>.SuccessResult(MapToDto(config));
    }

    internal static IntegrationConfigDto MapToDto(IntegrationConfig c) => new()
    {
        Id = c.Id,
        ChurchId = c.ChurchId,
        Service = c.Service,
        IsEnabled = c.IsEnabled,
        WebhookUrl = c.WebhookUrl,
        AdditionalConfig = c.AdditionalConfig,
        LastTestedAt = c.LastTestedAt,
        IsHealthy = c.IsHealthy,
        LastTestResult = c.LastTestResult,
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt
    };
}
