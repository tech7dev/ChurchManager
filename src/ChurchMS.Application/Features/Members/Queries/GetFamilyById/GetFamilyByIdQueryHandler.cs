using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Queries.GetFamilyById;

public class GetFamilyByIdQueryHandler(IFamilyRepository familyRepository)
    : IRequestHandler<GetFamilyByIdQuery, ApiResponse<FamilyDto>>
{
    public async Task<ApiResponse<FamilyDto>> Handle(
        GetFamilyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetWithMembersAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Family), request.Id);

        var dto = family.Adapt<FamilyDto>();
        dto.MemberCount = family.Members.Count;
        return ApiResponse<FamilyDto>.SuccessResult(dto);
    }
}
