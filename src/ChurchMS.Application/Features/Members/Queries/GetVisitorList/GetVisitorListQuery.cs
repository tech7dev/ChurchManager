using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Queries.GetVisitorList;

public class GetVisitorListQuery : IRequest<ApiResponse<PagedResult<VisitorDto>>>
{
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
