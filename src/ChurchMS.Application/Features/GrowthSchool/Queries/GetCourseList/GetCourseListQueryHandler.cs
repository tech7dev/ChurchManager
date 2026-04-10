using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Queries.GetCourseList;

public class GetCourseListQueryHandler(
    IRepository<GrowthSchoolCourse> courseRepository,
    IRepository<GrowthSchoolEnrollment> enrollmentRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetCourseListQuery, ApiResponse<IList<GrowthSchoolCourseDto>>>
{
    public async Task<ApiResponse<IList<GrowthSchoolCourseDto>>> Handle(
        GetCourseListQuery request,
        CancellationToken cancellationToken)
    {
        var courses = await courseRepository.FindAsync(c =>
            (!request.ActiveOnly || c.IsActive) &&
            (!request.Level.HasValue || c.Level == request.Level.Value),
            cancellationToken);

        var dtos = new List<GrowthSchoolCourseDto>();
        foreach (var course in courses)
        {
            var dto = course.Adapt<GrowthSchoolCourseDto>();
            dto.EnrollmentCount = await enrollmentRepository.CountAsync(
                e => e.CourseId == course.Id && e.Status == EnrollmentStatus.Active,
                cancellationToken);

            if (course.InstructorId.HasValue)
            {
                var instructor = await memberRepository.GetByIdAsync(course.InstructorId.Value, cancellationToken);
                if (instructor is not null)
                    dto.InstructorName = $"{instructor.FirstName} {instructor.LastName}";
            }

            dtos.Add(dto);
        }

        return ApiResponse<IList<GrowthSchoolCourseDto>>.SuccessResult(dtos);
    }
}
