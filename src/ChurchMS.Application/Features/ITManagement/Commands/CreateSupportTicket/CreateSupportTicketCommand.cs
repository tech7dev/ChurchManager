using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.CreateSupportTicket;

public record CreateSupportTicketCommand(
    string Title,
    string Description,
    TicketCategory Category,
    TicketPriority Priority
) : IRequest<ApiResponse<SupportTicketDto>>;
