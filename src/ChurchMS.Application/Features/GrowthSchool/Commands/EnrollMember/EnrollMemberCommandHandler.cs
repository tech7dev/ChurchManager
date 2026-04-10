using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Commands.EnrollMember;

public class EnrollMemberCommandHandler(
    IRepository<GrowthSchoolEnrollment> enrollmentRepository,
    IRepository<GrowthSchoolCourse> courseRepository,
    IRepository<Member> memberRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<EnrollMemberCommand, ApiResponse<GrowthEnrollmentDto>>
{
    public async Task<ApiResponse<GrowthEnrollmentDto>> Handle(
        EnrollMemberCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
            ?? throw new NotFoundException(nameof(GrowthSchoolCourse), request.CourseId);

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var existing = await enrollmentRepository.FindAsync(
            e => e.CourseId == request.CourseId &&
                 e.MemberId == request.MemberId &&
                 e.Status == EnrollmentStatus.Active,
            cancellationToken);

        if (existing.Count > 0)
            throw new BadRequestException("Member is already enrolled in this course.");

        var enrollment = new GrowthSchoolEnrollment
        {
            ChurchId = churchId,
            CourseId = request.CourseId,
            MemberId = request.MemberId,
            EnrolledDate = request.EnrolledDate,
            Status = EnrollmentStatus.Active,
            Notes = request.Notes
        };

        await enrollmentRepository.AddAsync(enrollment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = enrollment.Adapt<GrowthEnrollmentDto>();
        dto.CourseName = course.Name;
        dto.MemberName = $"{member.FirstName} {member.LastName}";

        return ApiResponse<GrowthEnrollmentDto>.SuccessResult(dto, "Member enrolled successfully.");
    }
}
