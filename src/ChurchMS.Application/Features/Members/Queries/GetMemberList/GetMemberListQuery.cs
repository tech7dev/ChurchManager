using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Queries.GetMemberList;

public class GetMemberListQuery : IRequest<ApiResponse<PagedResult<MemberListDto>>>
{
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
