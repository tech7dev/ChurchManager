using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Application.Features.ITManagement.Commands.CreateSupportTicket;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.UpdateTicketStatus;

public class UpdateTicketStatusCommandHandler(
    IRepository<SupportTicket> ticketRepository,
    IDateTimeService dateTimeService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateTicketStatusCommand, ApiResponse<SupportTicketDto>>
{
    public async Task<ApiResponse<SupportTicketDto>> Handle(
        UpdateTicketStatusCommand request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return ApiResponse<SupportTicketDto>.FailureResult("Support ticket not found.");

        ticket.Status = request.NewStatus;

        if (request.AssignedToUserId.HasValue)
            ticket.AssignedToUserId = request.AssignedToUserId;

        if (request.ResolutionNotes is not null)
            ticket.ResolutionNotes = request.ResolutionNotes;

        if (request.NewStatus is TicketStatus.Resolved or TicketStatus.Closed)
            ticket.ResolvedAt ??= dateTimeService.UtcNow;

        ticketRepository.Update(ticket);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<SupportTicketDto>.SuccessResult(CreateSupportTicketCommandHandler.MapToDto(ticket));
    }
}
