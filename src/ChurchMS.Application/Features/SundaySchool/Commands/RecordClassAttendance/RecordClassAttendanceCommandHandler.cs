using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Commands.RecordClassAttendance;

public class RecordClassAttendanceCommandHandler(
    IRepository<SundaySchoolAttendance> attendanceRepository,
    IRepository<SundaySchoolLesson> lessonRepository,
    IRepository<Member> memberRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<RecordClassAttendanceCommand, ApiResponse<ClassAttendanceDto>>
{
    public async Task<ApiResponse<ClassAttendanceDto>> Handle(
        RecordClassAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var lesson = await lessonRepository.GetByIdAsync(request.LessonId, cancellationToken)
            ?? throw new NotFoundException(nameof(SundaySchoolLesson), request.LessonId);

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var attendance = new SundaySchoolAttendance
        {
            ChurchId = churchId,
            LessonId = request.LessonId,
            MemberId = request.MemberId,
            Status = request.Status,
            Notes = request.Notes
        };

        await attendanceRepository.AddAsync(attendance, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = attendance.Adapt<ClassAttendanceDto>();
        dto.LessonTitle = lesson.Title;
        dto.MemberName = $"{member.FirstName} {member.LastName}";

        return ApiResponse<ClassAttendanceDto>.SuccessResult(dto, "Attendance recorded.");
    }
}
