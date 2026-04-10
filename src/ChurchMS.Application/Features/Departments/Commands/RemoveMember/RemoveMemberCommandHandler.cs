using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Application.Exceptions;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Commands.RemoveMember;

public class RemoveMemberCommandHandler(
    IRepository<DepartmentMember> deptMemberRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveMemberCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(
        RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        var deptMember = await deptMemberRepository.GetByIdAsync(request.DepartmentMemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(DepartmentMember), request.DepartmentMemberId);

        deptMember.IsActive = false;
        deptMember.LeftDate = request.LeftDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true);
    }
}
