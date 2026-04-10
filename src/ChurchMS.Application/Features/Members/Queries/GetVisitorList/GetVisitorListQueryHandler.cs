using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Queries.GetVisitorList;

public class GetVisitorListQueryHandler(IRepository<Visitor> visitorRepository)
    : IRequestHandler<GetVisitorListQuery, ApiResponse<PagedResult<VisitorDto>>>
{
    public async Task<ApiResponse<PagedResult<VisitorDto>>> Handle(
        GetVisitorListQuery request,
        CancellationToken cancellationToken)
    {
        var all = await visitorRepository.GetAllAsync(cancellationToken);

        var query = all.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLowerInvariant();
            query = query.Where(v =>
                v.FirstName.ToLower().Contains(term) ||
                v.LastName.ToLower().Contains(term) ||
                (v.Email != null && v.Email.ToLower().Contains(term)) ||
                (v.Phone != null && v.Phone.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(request.Status) &&
            Enum.TryParse<VisitorStatus>(request.Status, true, out var status))
        {
            query = query.Where(v => v.Status == status);
        }

        var total = query.Count();
        var items = query
            .OrderByDescending(v => v.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var result = new PagedResult<VisitorDto>
        {
            Items = items.Adapt<List<VisitorDto>>(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return ApiResponse<PagedResult<VisitorDto>>.SuccessResult(result);
    }
}
