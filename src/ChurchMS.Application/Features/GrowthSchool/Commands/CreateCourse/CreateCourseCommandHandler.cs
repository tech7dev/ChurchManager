using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Commands.CreateCourse;

public class CreateCourseCommandHandler(
    IRepository<GrowthSchoolCourse> courseRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateCourseCommand, ApiResponse<GrowthSchoolCourseDto>>
{
    public async Task<ApiResponse<GrowthSchoolCourseDto>> Handle(
        CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var course = new GrowthSchoolCourse
        {
            ChurchId = churchId,
            Name = request.Name,
            Description = request.Description,
            Level = request.Level,
            InstructorId = request.InstructorId,
            DurationWeeks = request.DurationWeeks,
            MaxCapacity = request.MaxCapacity,
            IsActive = true
        };

        await courseRepository.AddAsync(course, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<GrowthSchoolCourseDto>.SuccessResult(
            course.Adapt<GrowthSchoolCourseDto>(), "Course created successfully.");
    }
}
