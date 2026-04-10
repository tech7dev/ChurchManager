using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.AddTicketComment;

public record AddTicketCommentCommand(
    Guid TicketId,
    string Content,
    bool IsInternal
) : IRequest<ApiResponse<SupportTicketCommentDto>>;
