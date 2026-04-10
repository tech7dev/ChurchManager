using ChurchMS.Application.Features.ITManagement.Commands.CreateSupportTicket;
using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetSupportTicketList;

public class GetSupportTicketListQueryHandler(IRepository<SupportTicket> ticketRepository)
    : IRequestHandler<GetSupportTicketListQuery, ApiResponse<PagedResult<SupportTicketDto>>>
{
    public async Task<ApiResponse<PagedResult<SupportTicketDto>>> Handle(
        GetSupportTicketListQuery request, CancellationToken cancellationToken)
    {
        var all = await ticketRepository.FindAsync(
            t => (!request.Status.HasValue || t.Status == request.Status.Value)
              && (!request.Priority.HasValue || t.Priority == request.Priority.Value)
              && (!request.Category.HasValue || t.Category == request.Category.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(t => t.Priority)
            .ThenByDescending(t => t.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(CreateSupportTicketCommandHandler.MapToDto)
            .ToList();

        return ApiResponse<PagedResult<SupportTicketDto>>.SuccessResult(new PagedResult<SupportTicketDto>
        {
            Items = paged,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
