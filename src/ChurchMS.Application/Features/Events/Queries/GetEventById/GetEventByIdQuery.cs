using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Events.Queries.GetEventById;

public record GetEventByIdQuery(Guid Id) : IRequest<ApiResponse<EventDto>>;
