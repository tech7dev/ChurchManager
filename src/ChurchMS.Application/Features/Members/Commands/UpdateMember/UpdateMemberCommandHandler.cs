using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.UpdateMember;

public class UpdateMemberCommandHandler(
    IMemberRepository memberRepository,
    IFamilyRepository familyRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMemberCommand, ApiResponse<MemberDto>>
{
    public async Task<ApiResponse<MemberDto>> Handle(
        UpdateMemberCommand request,
        CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetWithDetailsAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.Id);

        if (request.FamilyId.HasValue)
        {
            var family = await familyRepository.GetByIdAsync(request.FamilyId.Value, cancellationToken);
            if (family is null)
                throw new NotFoundException(nameof(Family), request.FamilyId.Value);
        }

        request.Adapt(member);
        memberRepository.Update(member);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var updated = await memberRepository.GetWithDetailsAsync(member.Id, cancellationToken);
        var dto = updated!.Adapt<MemberDto>();
        if (updated.Family is not null)
            dto.FamilyName = updated.Family.Name;

        return ApiResponse<MemberDto>.SuccessResult(dto, "Member updated successfully.");
    }
}
