using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.CreateIntegrationConfig;

public record CreateIntegrationConfigCommand(
    IntegrationService Service,
    bool IsEnabled,
    string? ApiKey,
    string? ApiSecret,
    string? WebhookUrl,
    string? AdditionalConfig
) : IRequest<ApiResponse<IntegrationConfigDto>>;
