using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Commands.CreateLesson;

public class CreateLessonCommandHandler(
    IRepository<SundaySchoolLesson> lessonRepository,
    IRepository<SundaySchoolClass> classRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateLessonCommand, ApiResponse<SundaySchoolLessonDto>>
{
    public async Task<ApiResponse<SundaySchoolLessonDto>> Handle(
        CreateLessonCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var sundayClass = await classRepository.GetByIdAsync(request.ClassId, cancellationToken)
            ?? throw new NotFoundException(nameof(SundaySchoolClass), request.ClassId);

        var lesson = new SundaySchoolLesson
        {
            ChurchId = churchId,
            ClassId = request.ClassId,
            Title = request.Title,
            Description = request.Description,
            LessonNotes = request.LessonNotes,
            LessonDate = request.LessonDate,
            DurationMinutes = request.DurationMinutes
        };

        await lessonRepository.AddAsync(lesson, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = lesson.Adapt<SundaySchoolLessonDto>();
        dto.ClassName = sundayClass.Name;

        return ApiResponse<SundaySchoolLessonDto>.SuccessResult(dto, "Lesson created.");
    }
}
