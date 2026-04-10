using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Queries.GetLessonList;

public class GetLessonListQueryHandler(
    IRepository<SundaySchoolLesson> lessonRepository,
    IRepository<SundaySchoolClass> classRepository,
    IRepository<SundaySchoolAttendance> attendanceRepository)
    : IRequestHandler<GetLessonListQuery, ApiResponse<PagedResult<SundaySchoolLessonDto>>>
{
    public async Task<ApiResponse<PagedResult<SundaySchoolLessonDto>>> Handle(
        GetLessonListQuery request,
        CancellationToken cancellationToken)
    {
        var sundayClass = await classRepository.GetByIdAsync(request.ClassId, cancellationToken)
            ?? throw new NotFoundException(nameof(SundaySchoolClass), request.ClassId);

        var all = await lessonRepository.FindAsync(l =>
            l.ClassId == request.ClassId &&
            (!request.FromDate.HasValue || l.LessonDate >= request.FromDate.Value) &&
            (!request.ToDate.HasValue || l.LessonDate <= request.ToDate.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(l => l.LessonDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<SundaySchoolLessonDto>();
        foreach (var lesson in paged)
        {
            var dto = lesson.Adapt<SundaySchoolLessonDto>();
            dto.ClassName = sundayClass.Name;
            dto.AttendanceCount = await attendanceRepository.CountAsync(
                a => a.LessonId == lesson.Id, cancellationToken);
            dtos.Add(dto);
        }

        var result = new PagedResult<SundaySchoolLessonDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
        return ApiResponse<PagedResult<SundaySchoolLessonDto>>.SuccessResult(result);
    }
}
