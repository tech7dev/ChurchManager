using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.AddTicketComment;

public class AddTicketCommentCommandHandler(
    IRepository<SupportTicket> ticketRepository,
    IRepository<SupportTicketComment> commentRepository,
    ITenantService tenantService,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddTicketCommentCommand, ApiResponse<SupportTicketCommentDto>>
{
    public async Task<ApiResponse<SupportTicketCommentDto>> Handle(
        AddTicketCommentCommand request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return ApiResponse<SupportTicketCommentDto>.FailureResult("Support ticket not found.");

        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<SupportTicketCommentDto>.FailureResult("Church context required.");

        var userId = currentUserService.GetUserId();
        if (!userId.HasValue)
            return ApiResponse<SupportTicketCommentDto>.FailureResult("User context required.");

        var comment = new SupportTicketComment
        {
            ChurchId = churchId.Value,
            TicketId = request.TicketId,
            AuthorId = userId.Value,
            Content = request.Content,
            IsInternal = request.IsInternal
        };

        await commentRepository.AddAsync(comment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<SupportTicketCommentDto>.SuccessResult(new SupportTicketCommentDto
        {
            Id = comment.Id,
            TicketId = comment.TicketId,
            AuthorId = comment.AuthorId,
            Content = comment.Content,
            IsInternal = comment.IsInternal,
            CreatedAt = comment.CreatedAt
        });
    }
}
