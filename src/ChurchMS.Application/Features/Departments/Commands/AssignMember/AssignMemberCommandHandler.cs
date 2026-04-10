using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Commands.AssignMember;

public class AssignMemberCommandHandler(
    IRepository<Department> departmentRepository,
    IRepository<DepartmentMember> deptMemberRepository,
    IRepository<Member> memberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AssignMemberCommand, ApiResponse<DepartmentMemberDto>>
{
    public async Task<ApiResponse<DepartmentMemberDto>> Handle(
        AssignMemberCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()!.Value;

        var department = await departmentRepository.GetByIdAsync(request.DepartmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Department), request.DepartmentId);

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        // Prevent duplicate active membership
        var existing = await deptMemberRepository.FindAsync(
            dm => dm.DepartmentId == request.DepartmentId
               && dm.MemberId == request.MemberId
               && dm.IsActive,
            cancellationToken);

        if (existing.Count > 0)
            return ApiResponse<DepartmentMemberDto>.FailureResult("Member is already an active member of this department.");

        var deptMember = new DepartmentMember
        {
            ChurchId = churchId,
            DepartmentId = request.DepartmentId,
            MemberId = request.MemberId,
            Role = request.Role,
            JoinedDate = request.JoinedDate,
            IsActive = true,
            Notes = request.Notes
        };

        await deptMemberRepository.AddAsync(deptMember, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<DepartmentMemberDto>.SuccessResult(new DepartmentMemberDto
        {
            Id = deptMember.Id,
            DepartmentId = department.Id,
            DepartmentName = department.Name,
            MemberId = member.Id,
            MemberName = $"{member.FirstName} {member.LastName}",
            Role = deptMember.Role,
            JoinedDate = deptMember.JoinedDate,
            IsActive = true,
            Notes = deptMember.Notes,
            CreatedAt = deptMember.CreatedAt
        });
    }
}
