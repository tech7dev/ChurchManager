using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandHandler(
    IRepository<ChurchEvent> eventRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateEventCommand, ApiResponse<EventDto>>
{
    public async Task<ApiResponse<EventDto>> Handle(
        CreateEventCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var churchEvent = new ChurchEvent
        {
            ChurchId = churchId,
            Title = request.Title,
            Description = request.Description,
            Type = request.Type,
            Status = EventStatus.Draft,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            Location = request.Location,
            OnlineLink = request.OnlineLink,
            RequiresRegistration = request.RequiresRegistration,
            MaxAttendees = request.MaxAttendees,
            RegistrationFee = request.RegistrationFee,
            Currency = request.Currency,
            IsRecurring = request.IsRecurring,
            RecurrenceFrequency = request.RecurrenceFrequency,
            RecurrenceEndDate = request.RecurrenceEndDate,
            QrCodeValue = $"EVENT:{churchId}:{Guid.NewGuid()}"
        };

        await eventRepository.AddAsync(churchEvent, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<EventDto>.SuccessResult(churchEvent.Adapt<EventDto>(), "Event created successfully.");
    }
}
