using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.CreateMember;

public class CreateMemberCommandHandler(
    IMemberRepository memberRepository,
    IFamilyRepository familyRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateMemberCommand, ApiResponse<MemberDto>>
{
    public async Task<ApiResponse<MemberDto>> Handle(
        CreateMemberCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        // Validate family exists in same church
        if (request.FamilyId.HasValue)
        {
            var family = await familyRepository.GetByIdAsync(request.FamilyId.Value, cancellationToken);
            if (family is null)
                throw new NotFoundException(nameof(Family), request.FamilyId.Value);
        }

        var member = request.Adapt<Member>();
        member.ChurchId = churchId;
        member.QrCodeValue = $"MEMBER:{churchId}:{Guid.NewGuid()}";
        member.MembershipNumber = await memberRepository.GenerateNextMembershipNumberAsync(churchId, cancellationToken);

        await memberRepository.AddAsync(member, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with family details for DTO mapping
        var saved = await memberRepository.GetWithDetailsAsync(member.Id, cancellationToken);
        var dto = saved!.Adapt<MemberDto>();
        if (saved.Family is not null)
            dto.FamilyName = saved.Family.Name;

        return ApiResponse<MemberDto>.SuccessResult(dto, "Member created successfully.");
    }
}
