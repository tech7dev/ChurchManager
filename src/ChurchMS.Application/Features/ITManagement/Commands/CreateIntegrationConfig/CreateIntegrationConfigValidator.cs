using FluentValidation;

namespace ChurchMS.Application.Features.ITManagement.Commands.CreateIntegrationConfig;

public class CreateIntegrationConfigValidator : AbstractValidator<CreateIntegrationConfigCommand>
{
    public CreateIntegrationConfigValidator()
    {
        RuleFor(x => x.Service).IsInEnum();
        RuleFor(x => x.WebhookUrl).MaximumLength(500).When(x => x.WebhookUrl is not null);
        RuleFor(x => x.ApiKey).MaximumLength(500).When(x => x.ApiKey is not null);
        RuleFor(x => x.ApiSecret).MaximumLength(500).When(x => x.ApiSecret is not null);
    }
}
