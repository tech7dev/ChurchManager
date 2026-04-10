using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Queries.GetDepartmentList;

public record GetDepartmentListQuery(bool ActiveOnly = true) : IRequest<ApiResponse<List<DepartmentDto>>>;
