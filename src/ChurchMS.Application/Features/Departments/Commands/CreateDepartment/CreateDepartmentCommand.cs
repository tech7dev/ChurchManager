using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(
    string Name,
    string? Description,
    Guid? LeaderId,
    string? Color
) : IRequest<ApiResponse<DepartmentDto>>;
