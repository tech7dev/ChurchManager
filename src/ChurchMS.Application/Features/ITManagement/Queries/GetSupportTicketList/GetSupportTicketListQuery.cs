using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetSupportTicketList;

public record GetSupportTicketListQuery(
    TicketStatus? Status = null,
    TicketPriority? Priority = null,
    TicketCategory? Category = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<SupportTicketDto>>>;
