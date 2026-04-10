using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Queries.GetMemberById;

public class GetMemberByIdQueryHandler(IMemberRepository memberRepository)
    : IRequestHandler<GetMemberByIdQuery, ApiResponse<MemberDto>>
{
    public async Task<ApiResponse<MemberDto>> Handle(
        GetMemberByIdQuery request,
        CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetWithDetailsAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.Id);

        var dto = member.Adapt<MemberDto>();
        if (member.Family is not null)
            dto.FamilyName = member.Family.Name;

        return ApiResponse<MemberDto>.SuccessResult(dto);
    }
}
