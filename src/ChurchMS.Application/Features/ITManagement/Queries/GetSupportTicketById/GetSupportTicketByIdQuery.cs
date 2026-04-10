using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetSupportTicketById;

public record GetSupportTicketByIdQuery(Guid Id) : IRequest<ApiResponse<SupportTicketDto>>;
