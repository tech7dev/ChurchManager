using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Events.Commands.PublishEvent;

public class PublishEventCommandHandler(
    IRepository<ChurchEvent> eventRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PublishEventCommand, ApiResponse<EventDto>>
{
    public async Task<ApiResponse<EventDto>> Handle(
        PublishEventCommand request,
        CancellationToken cancellationToken)
    {
        var churchEvent = await eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException(nameof(ChurchEvent), request.EventId);

        if (churchEvent.Status != EventStatus.Draft)
            throw new BadRequestException("Only draft events can be published.");

        churchEvent.Status = EventStatus.Published;
        eventRepository.Update(churchEvent);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<EventDto>.SuccessResult(churchEvent.Adapt<EventDto>(), "Event published.");
    }
}
