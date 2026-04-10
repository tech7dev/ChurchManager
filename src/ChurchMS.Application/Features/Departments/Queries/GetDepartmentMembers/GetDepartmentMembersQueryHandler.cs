using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Queries.GetDepartmentMembers;

public class GetDepartmentMembersQueryHandler(
    IRepository<DepartmentMember> deptMemberRepository,
    IRepository<Department> departmentRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetDepartmentMembersQuery, ApiResponse<PagedResult<DepartmentMemberDto>>>
{
    public async Task<ApiResponse<PagedResult<DepartmentMemberDto>>> Handle(
        GetDepartmentMembersQuery request, CancellationToken cancellationToken)
    {
        var all = request.ActiveOnly
            ? await deptMemberRepository.FindAsync(
                dm => dm.DepartmentId == request.DepartmentId && dm.IsActive, cancellationToken)
            : await deptMemberRepository.FindAsync(
                dm => dm.DepartmentId == request.DepartmentId, cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderBy(dm => dm.JoinedDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dept = await departmentRepository.GetByIdAsync(request.DepartmentId, cancellationToken);

        var dtos = new List<DepartmentMemberDto>();
        foreach (var dm in paged)
        {
            var member = await memberRepository.GetByIdAsync(dm.MemberId, cancellationToken);
            dtos.Add(new DepartmentMemberDto
            {
                Id = dm.Id,
                DepartmentId = dm.DepartmentId,
                DepartmentName = dept?.Name ?? "",
                MemberId = dm.MemberId,
                MemberName = member is not null ? $"{member.FirstName} {member.LastName}" : "",
                Role = dm.Role,
                JoinedDate = dm.JoinedDate,
                LeftDate = dm.LeftDate,
                IsActive = dm.IsActive,
                Notes = dm.Notes,
                CreatedAt = dm.CreatedAt
            });
        }

        return ApiResponse<PagedResult<DepartmentMemberDto>>.SuccessResult(new PagedResult<DepartmentMemberDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
