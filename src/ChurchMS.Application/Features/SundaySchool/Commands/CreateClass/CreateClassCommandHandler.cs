using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Commands.CreateClass;

public class CreateClassCommandHandler(
    IRepository<SundaySchoolClass> classRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateClassCommand, ApiResponse<SundaySchoolClassDto>>
{
    public async Task<ApiResponse<SundaySchoolClassDto>> Handle(
        CreateClassCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var sundayClass = new SundaySchoolClass
        {
            ChurchId = churchId,
            Name = request.Name,
            Description = request.Description,
            Level = request.Level,
            TeacherId = request.TeacherId,
            MinAge = request.MinAge,
            MaxAge = request.MaxAge,
            MaxCapacity = request.MaxCapacity,
            IsActive = true
        };

        await classRepository.AddAsync(sundayClass, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<SundaySchoolClassDto>.SuccessResult(
            sundayClass.Adapt<SundaySchoolClassDto>(), "Class created successfully.");
    }
}
