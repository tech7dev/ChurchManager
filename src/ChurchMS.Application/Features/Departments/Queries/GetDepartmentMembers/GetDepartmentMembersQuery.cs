using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Queries.GetDepartmentMembers;

public record GetDepartmentMembersQuery(
    Guid DepartmentId,
    bool ActiveOnly = true,
    int Page = 1,
    int PageSize = 50
) : IRequest<ApiResponse<PagedResult<DepartmentMemberDto>>>;
