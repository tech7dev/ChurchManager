using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Events.Commands.PublishEvent;

public record PublishEventCommand(Guid EventId) : IRequest<ApiResponse<EventDto>>;
