using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler(
    IRepository<Department> departmentRepository,
    IRepository<Member> memberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateDepartmentCommand, ApiResponse<DepartmentDto>>
{
    public async Task<ApiResponse<DepartmentDto>> Handle(
        CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()!.Value;

        var department = new Department
        {
            ChurchId = churchId,
            Name = request.Name,
            Description = request.Description,
            LeaderId = request.LeaderId,
            Color = request.Color,
            IsActive = true
        };

        await departmentRepository.AddAsync(department, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? leaderName = null;
        if (request.LeaderId.HasValue)
        {
            var leader = await memberRepository.GetByIdAsync(request.LeaderId.Value, cancellationToken);
            leaderName = leader is not null ? $"{leader.FirstName} {leader.LastName}" : null;
        }

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
            MemberCount = 0,
            CreatedAt = department.CreatedAt
        });
    }
}
