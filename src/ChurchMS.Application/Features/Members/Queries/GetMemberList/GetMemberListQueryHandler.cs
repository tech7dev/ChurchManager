using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Queries.GetMemberList;

public class GetMemberListQueryHandler(IMemberRepository memberRepository)
    : IRequestHandler<GetMemberListQuery, ApiResponse<PagedResult<MemberListDto>>>
{
    public async Task<ApiResponse<PagedResult<MemberListDto>>> Handle(
        GetMemberListQuery request,
        CancellationToken cancellationToken)
    {
        var (items, total) = await memberRepository.GetPagedAsync(
            request.SearchTerm, request.Status, request.Page, request.PageSize, cancellationToken);

        var result = new PagedResult<MemberListDto>
        {
            Items = items.Adapt<List<MemberListDto>>(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return ApiResponse<PagedResult<MemberListDto>>.SuccessResult(result);
    }
}
