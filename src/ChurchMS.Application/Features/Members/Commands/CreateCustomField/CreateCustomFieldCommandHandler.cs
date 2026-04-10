using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.CreateCustomField;

public class CreateCustomFieldCommandHandler(
    IRepository<CustomField> customFieldRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateCustomFieldCommand, ApiResponse<CustomFieldDto>>
{
    public async Task<ApiResponse<CustomFieldDto>> Handle(
        CreateCustomFieldCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var field = request.Adapt<CustomField>();
        field.ChurchId = churchId;

        await customFieldRepository.AddAsync(field, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<CustomFieldDto>.SuccessResult(field.Adapt<CustomFieldDto>(), "Custom field created successfully.");
    }
}
