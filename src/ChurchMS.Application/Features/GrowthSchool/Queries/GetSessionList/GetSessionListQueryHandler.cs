using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Queries.GetSessionList;

public class GetSessionListQueryHandler(
    IRepository<GrowthSchoolSession> sessionRepository,
    IRepository<GrowthSchoolCourse> courseRepository,
    IRepository<GrowthSchoolAttendance> attendanceRepository)
    : IRequestHandler<GetSessionListQuery, ApiResponse<PagedResult<GrowthSchoolSessionDto>>>
{
    public async Task<ApiResponse<PagedResult<GrowthSchoolSessionDto>>> Handle(
        GetSessionListQuery request,
        CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
            ?? throw new NotFoundException(nameof(GrowthSchoolCourse), request.CourseId);

        var all = await sessionRepository.FindAsync(s =>
            s.CourseId == request.CourseId &&
            (!request.FromDate.HasValue || s.SessionDate >= request.FromDate.Value) &&
            (!request.ToDate.HasValue || s.SessionDate <= request.ToDate.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(s => s.SessionDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<GrowthSchoolSessionDto>();
        foreach (var session in paged)
        {
            var dto = session.Adapt<GrowthSchoolSessionDto>();
            dto.CourseName = course.Name;
            dto.AttendanceCount = await attendanceRepository.CountAsync(
                a => a.SessionId == session.Id, cancellationToken);
            dtos.Add(dto);
        }

        var result = new PagedResult<GrowthSchoolSessionDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return ApiResponse<PagedResult<GrowthSchoolSessionDto>>.SuccessResult(result);
    }
}
