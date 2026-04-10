using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Events.Commands.CreateEvent;

public record CreateEventCommand(
    string Title,
    string? Description,
    EventType Type,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string? Location,
    string? OnlineLink,
    bool RequiresRegistration,
    int? MaxAttendees,
    decimal? RegistrationFee,
    string? Currency,
    bool IsRecurring,
    RecurrenceFrequency? RecurrenceFrequency,
    DateOnly? RecurrenceEndDate) : IRequest<ApiResponse<EventDto>>;
