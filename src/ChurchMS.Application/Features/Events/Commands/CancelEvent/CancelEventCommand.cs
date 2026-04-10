using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Events.Commands.CancelEvent;

public record CancelEventCommand(Guid EventId) : IRequest<ApiResponse<EventDto>>;
