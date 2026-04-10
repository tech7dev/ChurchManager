using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Commands.CreateSession;

public class CreateSessionCommandHandler(
    IRepository<GrowthSchoolSession> sessionRepository,
    IRepository<GrowthSchoolCourse> courseRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateSessionCommand, ApiResponse<GrowthSchoolSessionDto>>
{
    public async Task<ApiResponse<GrowthSchoolSessionDto>> Handle(
        CreateSessionCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
            ?? throw new NotFoundException(nameof(GrowthSchoolCourse), request.CourseId);

        var session = new GrowthSchoolSession
        {
            ChurchId = churchId,
            CourseId = request.CourseId,
            Title = request.Title,
            Description = request.Description,
            SessionNotes = request.SessionNotes,
            SessionDate = request.SessionDate,
            DurationMinutes = request.DurationMinutes
        };

        await sessionRepository.AddAsync(session, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = session.Adapt<GrowthSchoolSessionDto>();
        dto.CourseName = course.Name;

        return ApiResponse<GrowthSchoolSessionDto>.SuccessResult(dto, "Session created.");
    }
}
