using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Events.Queries.GetEventRegistrations;

public record GetEventRegistrationsQuery(
    Guid EventId,
    RegistrationStatus? Status = null,
    int Page = 1,
    int PageSize = 50) : IRequest<ApiResponse<PagedResult<EventRegistrationDto>>>;
