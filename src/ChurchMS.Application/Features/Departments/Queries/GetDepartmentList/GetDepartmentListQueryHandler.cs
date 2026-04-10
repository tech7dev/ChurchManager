using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Queries.GetDepartmentList;

public class GetDepartmentListQueryHandler(
    IRepository<Department> departmentRepository,
    IRepository<Member> memberRepository,
    IRepository<DepartmentMember> deptMemberRepository)
    : IRequestHandler<GetDepartmentListQuery, ApiResponse<List<DepartmentDto>>>
{
    public async Task<ApiResponse<List<DepartmentDto>>> Handle(
        GetDepartmentListQuery request, CancellationToken cancellationToken)
    {
        var departments = request.ActiveOnly
            ? await departmentRepository.FindAsync(d => d.IsActive, cancellationToken)
            : await departmentRepository.GetAllAsync(cancellationToken);

        var ordered = departments.OrderBy(d => d.Name).ToList();

        var leaderIds = ordered.Where(d => d.LeaderId.HasValue).Select(d => d.LeaderId!.Value).Distinct().ToList();
        var leaders = new Dictionary<Guid, string>();
        foreach (var lid in leaderIds)
        {
            var leader = await memberRepository.GetByIdAsync(lid, cancellationToken);
            if (leader is not null)
                leaders[lid] = $"{leader.FirstName} {leader.LastName}";
        }

        var dtos = new List<DepartmentDto>();
        foreach (var d in ordered)
        {
            var memberCount = await deptMemberRepository.CountAsync(
                dm => dm.DepartmentId == d.Id && dm.IsActive, cancellationToken);

            dtos.Add(new DepartmentDto
            {
                Id = d.Id,
                ChurchId = d.ChurchId,
                Name = d.Name,
                Description = d.Description,
                LeaderId = d.LeaderId,
                LeaderName = d.LeaderId.HasValue && leaders.TryGetValue(d.LeaderId.Value, out var ln) ? ln : null,
                Color = d.Color,
                IsActive = d.IsActive,
                MemberCount = memberCount,
                CreatedAt = d.CreatedAt
            });
        }

        return ApiResponse<List<DepartmentDto>>.SuccessResult(dtos);
    }
}
