using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.UpdateIntegrationConfig;

public record UpdateIntegrationConfigCommand(
    Guid ConfigId,
    bool IsEnabled,
    string? ApiKey,
    string? ApiSecret,
    string? WebhookUrl,
    string? AdditionalConfig
) : IRequest<ApiResponse<IntegrationConfigDto>>;
