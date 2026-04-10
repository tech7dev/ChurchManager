using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Queries.GetClassList;

public class GetClassListQueryHandler(
    IRepository<SundaySchoolClass> classRepository,
    IRepository<SundaySchoolEnrollment> enrollmentRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetClassListQuery, ApiResponse<IList<SundaySchoolClassDto>>>
{
    public async Task<ApiResponse<IList<SundaySchoolClassDto>>> Handle(
        GetClassListQuery request,
        CancellationToken cancellationToken)
    {
        var classes = await classRepository.FindAsync(c =>
            (!request.ActiveOnly || c.IsActive) &&
            (!request.Level.HasValue || c.Level == request.Level.Value),
            cancellationToken);

        var dtos = new List<SundaySchoolClassDto>();
        foreach (var c in classes)
        {
            var dto = c.Adapt<SundaySchoolClassDto>();
            dto.EnrollmentCount = await enrollmentRepository.CountAsync(
                e => e.ClassId == c.Id && e.Status == Domain.Enums.EnrollmentStatus.Active,
                cancellationToken);

            if (c.TeacherId.HasValue)
            {
                var teacher = await memberRepository.GetByIdAsync(c.TeacherId.Value, cancellationToken);
                if (teacher is not null)
                    dto.TeacherName = $"{teacher.FirstName} {teacher.LastName}";
            }

            dtos.Add(dto);
        }

        return ApiResponse<IList<SundaySchoolClassDto>>.SuccessResult(dtos);
    }
}
