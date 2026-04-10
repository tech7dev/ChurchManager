using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Events.Queries.GetEventList;

public class GetEventListQueryHandler(
    IRepository<ChurchEvent> eventRepository,
    IRepository<EventRegistration> registrationRepository)
    : IRequestHandler<GetEventListQuery, ApiResponse<PagedResult<EventListDto>>>
{
    public async Task<ApiResponse<PagedResult<EventListDto>>> Handle(
        GetEventListQuery request,
        CancellationToken cancellationToken)
    {
        var all = await eventRepository.FindAsync(e =>
            (!request.Type.HasValue || e.Type == request.Type.Value) &&
            (!request.Status.HasValue || e.Status == request.Status.Value) &&
            (!request.FromDate.HasValue || e.StartDateTime >= request.FromDate.Value) &&
            (!request.ToDate.HasValue || e.StartDateTime <= request.ToDate.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderBy(e => e.StartDateTime)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<EventListDto>();
        foreach (var ev in paged)
        {
            var dto = ev.Adapt<EventListDto>();
            dto.RegistrationCount = await registrationRepository.CountAsync(
                r => r.EventId == ev.Id && r.Status != Domain.Enums.RegistrationStatus.Cancelled,
                cancellationToken);
            dtos.Add(dto);
        }

        var result = new PagedResult<EventListDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
        return ApiResponse<PagedResult<EventListDto>>.SuccessResult(result);
    }
}
