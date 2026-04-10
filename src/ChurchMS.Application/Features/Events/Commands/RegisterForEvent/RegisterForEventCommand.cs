using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Events.Commands.RegisterForEvent;

public record RegisterForEventCommand(
    Guid EventId,
    Guid? MemberId,
    string? GuestName,
    string? GuestEmail,
    string? GuestPhone,
    string? Notes) : IRequest<ApiResponse<EventRegistrationDto>>;
