using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Events.Commands.CancelEvent;

public class CancelEventCommandHandler(
    IRepository<ChurchEvent> eventRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CancelEventCommand, ApiResponse<EventDto>>
{
    public async Task<ApiResponse<EventDto>> Handle(
        CancelEventCommand request,
        CancellationToken cancellationToken)
    {
        var churchEvent = await eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(ChurchEvent), request.EventId);

        if (churchEvent.Status == EventStatus.Completed)
            throw new BadRequestException("Completed events cannot be cancelled.");

        churchEvent.Status = EventStatus.Cancelled;
        eventRepository.Update(churchEvent);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<EventDto>.SuccessResult(churchEvent.Adapt<EventDto>(), "Event cancelled.");
    }
}
