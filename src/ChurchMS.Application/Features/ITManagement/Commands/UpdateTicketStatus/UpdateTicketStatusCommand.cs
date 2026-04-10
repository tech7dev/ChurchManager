using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.UpdateTicketStatus;

public record UpdateTicketStatusCommand(
    Guid TicketId,
    TicketStatus NewStatus,
    Guid? AssignedToUserId,
    string? ResolutionNotes
) : IRequest<ApiResponse<SupportTicketDto>>;
