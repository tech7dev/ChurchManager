using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Application.Exceptions;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Events.Queries.GetEventRegistrations;

public class GetEventRegistrationsQueryHandler(
    IRepository<EventRegistration> registrationRepository,
    IRepository<ChurchEvent> eventRepository)
    : IRequestHandler<GetEventRegistrationsQuery, ApiResponse<PagedResult<EventRegistrationDto>>>
{
    public async Task<ApiResponse<PagedResult<EventRegistrationDto>>> Handle(
        GetEventRegistrationsQuery request,
        CancellationToken cancellationToken)
    {
        var churchEvent = await eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(ChurchEvent), request.EventId);

        var all = await registrationRepository.FindAsync(r =>
            r.EventId == request.EventId &&
            (!request.Status.HasValue || r.Status == request.Status.Value),
            cancellationToken);

        var totalCount = all.Count;
        var items = all
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r =>
            {
                var dto = r.Adapt<EventRegistrationDto>();
                dto.EventTitle = churchEvent.Title;
                return dto;
            })
            .ToList();

        var result = new PagedResult<EventRegistrationDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
        return ApiResponse<PagedResult<EventRegistrationDto>>.SuccessResult(result);
    }
}
