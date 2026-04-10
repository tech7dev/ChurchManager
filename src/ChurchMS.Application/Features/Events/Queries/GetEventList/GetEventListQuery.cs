using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Events.Queries.GetEventList;

public record GetEventListQuery(
    EventType? Type = null,
    EventStatus? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int Page = 1,
    int PageSize = 10) : IRequest<ApiResponse<PagedResult<EventListDto>>>;
