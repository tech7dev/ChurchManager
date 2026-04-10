using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler(
    IRepository<Department> departmentRepository,
    IRepository<Member> memberRepository,
    IRepository<DepartmentMember> deptMemberRepository)
    : IRequestHandler<GetDepartmentByIdQuery, ApiResponse<DepartmentDto>>
{
    public async Task<ApiResponse<DepartmentDto>> Handle(
        GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Department), request.Id);

        string? leaderName = null;
        if (department.LeaderId.HasValue)
        {
            var leader = await memberRepository.GetByIdAsync(department.LeaderId.Value, cancellationToken);
            leaderName = leader is not null ? $"{leader.FirstName} {leader.LastName}" : null;
        }

        var memberCount = await deptMemberRepository.CountAsync(
            dm => dm.DepartmentId == department.Id && dm.IsActive, cancellationToken);

        return ApiResponse<DepartmentDto>.SuccessResult(new DepartmentDto
        {
            Id = department.Id,
            ChurchId = department.ChurchId,
            Name = department.Name,
            Description = department.Description,
            LeaderId = department.LeaderId,
            LeaderName = leaderName,
            Color = department.Color,
            IsActive = department.IsActive,
            MemberCount = memberCount,
            CreatedAt = department.CreatedAt
        });
    }
}
