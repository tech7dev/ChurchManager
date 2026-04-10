using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Commands.EnrollMember;

public class EnrollMemberCommandHandler(
    IRepository<SundaySchoolEnrollment> enrollmentRepository,
    IRepository<SundaySchoolClass> classRepository,
    IRepository<Member> memberRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<EnrollMemberCommand, ApiResponse<EnrollmentDto>>
{
    public async Task<ApiResponse<EnrollmentDto>> Handle(
        EnrollMemberCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var sundayClass = await classRepository.GetByIdAsync(request.ClassId, cancellationToken)
            ?? throw new NotFoundException(nameof(SundaySchoolClass), request.ClassId);

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        // Prevent duplicate active enrollment
        var existing = await enrollmentRepository.FindAsync(
            e => e.ClassId == request.ClassId &&
                 e.MemberId == request.MemberId &&
                 e.Status == EnrollmentStatus.Active,
            cancellationToken);

        if (existing.Count > 0)
            throw new BadRequestException("Member is already enrolled in this class.");

        var enrollment = new SundaySchoolEnrollment
        {
            ChurchId = churchId,
            ClassId = request.ClassId,
            MemberId = request.MemberId,
            EnrolledDate = request.EnrolledDate,
            Status = EnrollmentStatus.Active,
            Notes = request.Notes
        };

        await enrollmentRepository.AddAsync(enrollment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = enrollment.Adapt<EnrollmentDto>();
        dto.ClassName = sundayClass.Name;
        dto.MemberName = $"{member.FirstName} {member.LastName}";

        return ApiResponse<EnrollmentDto>.SuccessResult(dto, "Member enrolled successfully.");
    }
}
