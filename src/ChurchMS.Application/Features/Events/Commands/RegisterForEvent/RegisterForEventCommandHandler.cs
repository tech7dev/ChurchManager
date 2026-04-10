using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Events.Commands.RegisterForEvent;

public class RegisterForEventCommandHandler(
    IRepository<EventRegistration> registrationRepository,
    IRepository<ChurchEvent> eventRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<RegisterForEventCommand, ApiResponse<EventRegistrationDto>>
{
    public async Task<ApiResponse<EventRegistrationDto>> Handle(
        RegisterForEventCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var churchEvent = await eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(ChurchEvent), request.EventId);

        if (churchEvent.Status != EventStatus.Published)
            throw new BadRequestException("Registrations are only accepted for published events.");

        // Check capacity
        if (churchEvent.MaxAttendees.HasValue)
        {
            var currentCount = await registrationRepository.CountAsync(
                r => r.EventId == request.EventId &&
                     r.Status != RegistrationStatus.Cancelled,
                cancellationToken);

            if (currentCount >= churchEvent.MaxAttendees.Value)
                throw new BadRequestException("Event is at full capacity.");
        }

        var registration = new EventRegistration
        {
            ChurchId = churchId,
            EventId = request.EventId,
            MemberId = request.MemberId,
            GuestName = request.GuestName,
            GuestEmail = request.GuestEmail,
            GuestPhone = request.GuestPhone,
            Status = RegistrationStatus.Confirmed,
            RegistrationCode = $"REG-{Guid.NewGuid().ToString()[..8].ToUpper()}",
            Notes = request.Notes
        };

        await registrationRepository.AddAsync(registration, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = registration.Adapt<EventRegistrationDto>();
        dto.EventTitle = churchEvent.Title;

        return ApiResponse<EventRegistrationDto>.SuccessResult(dto, "Registration confirmed.");
    }
}
