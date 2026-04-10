using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetSystemLogList;

public class GetSystemLogListQueryHandler(IRepository<SystemLog> logRepository)
    : IRequestHandler<GetSystemLogListQuery, ApiResponse<PagedResult<SystemLogDto>>>
{
    public async Task<ApiResponse<PagedResult<SystemLogDto>>> Handle(
        GetSystemLogListQuery request, CancellationToken cancellationToken)
    {
        var all = await logRepository.FindAsync(
            l => (request.Action == null || l.Action.Contains(request.Action))
              && (request.EntityType == null || l.EntityType == request.EntityType)
              && (request.Level == null || l.Level == request.Level)
              && (!request.From.HasValue || l.CreatedAt >= request.From.Value)
              && (!request.To.HasValue || l.CreatedAt <= request.To.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(l => l.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(l => new SystemLogDto
            {
                Id = l.Id,
                ChurchId = l.ChurchId,
                Action = l.Action,
                EntityType = l.EntityType,
                EntityId = l.EntityId,
                Details = l.Details,
                UserId = l.UserId,
                IpAddress = l.IpAddress,
                Level = l.Level,
                CreatedAt = l.CreatedAt
            })
            .ToList();

        return ApiResponse<PagedResult<SystemLogDto>>.SuccessResult(new PagedResult<SystemLogDto>
        {
            Items = paged,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
