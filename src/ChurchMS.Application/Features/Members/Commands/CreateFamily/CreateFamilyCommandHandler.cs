using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.CreateFamily;

public class CreateFamilyCommandHandler(
    IFamilyRepository familyRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateFamilyCommand, ApiResponse<FamilyDto>>
{
    public async Task<ApiResponse<FamilyDto>> Handle(
        CreateFamilyCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var family = request.Adapt<Family>();
        family.ChurchId = churchId;

        await familyRepository.AddAsync(family, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = family.Adapt<FamilyDto>();
        return ApiResponse<FamilyDto>.SuccessResult(dto, "Family created successfully.");
    }
}
