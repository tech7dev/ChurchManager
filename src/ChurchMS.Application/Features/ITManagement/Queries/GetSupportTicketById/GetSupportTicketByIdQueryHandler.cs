using ChurchMS.Application.Features.ITManagement.Commands.CreateSupportTicket;
using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetSupportTicketById;

public class GetSupportTicketByIdQueryHandler(
    IRepository<SupportTicket> ticketRepository,
    IRepository<SupportTicketComment> commentRepository)
    : IRequestHandler<GetSupportTicketByIdQuery, ApiResponse<SupportTicketDto>>
{
    public async Task<ApiResponse<SupportTicketDto>> Handle(
        GetSupportTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(request.Id, cancellationToken);
        if (ticket is null)
            return ApiResponse<SupportTicketDto>.FailureResult("Support ticket not found.");

        var comments = await commentRepository.FindAsync(
            c => c.TicketId == request.Id, cancellationToken);

        var dto = CreateSupportTicketCommandHandler.MapToDto(ticket);
        dto.Comments = comments
            .OrderBy(c => c.CreatedAt)
            .Select(c => new SupportTicketCommentDto
            {
                Id = c.Id,
                TicketId = c.TicketId,
                AuthorId = c.AuthorId,
                Content = c.Content,
                IsInternal = c.IsInternal,
                CreatedAt = c.CreatedAt
            })
            .ToList();

        return ApiResponse<SupportTicketDto>.SuccessResult(dto);
    }
}
