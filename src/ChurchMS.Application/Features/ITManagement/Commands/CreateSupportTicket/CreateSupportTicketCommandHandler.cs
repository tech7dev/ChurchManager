using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Commands.CreateSupportTicket;

public class CreateSupportTicketCommandHandler(
    IRepository<SupportTicket> ticketRepository,
    ITenantService tenantService,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSupportTicketCommand, ApiResponse<SupportTicketDto>>
{
    public async Task<ApiResponse<SupportTicketDto>> Handle(
        CreateSupportTicketCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<SupportTicketDto>.FailureResult("Church context required.");

        var userId = currentUserService.GetUserId();
        if (!userId.HasValue)
            return ApiResponse<SupportTicketDto>.FailureResult("User context required.");

        var ticket = new SupportTicket
        {
            ChurchId = churchId.Value,
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Priority = request.Priority,
            Status = TicketStatus.Open,
            SubmittedByUserId = userId.Value
        };

        await ticketRepository.AddAsync(ticket, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<SupportTicketDto>.SuccessResult(MapToDto(ticket));
    }

    internal static SupportTicketDto MapToDto(SupportTicket t) => new()
    {
        Id = t.Id,
        ChurchId = t.ChurchId,
        Title = t.Title,
        Description = t.Description,
        Category = t.Category,
        Priority = t.Priority,
        Status = t.Status,
        SubmittedByUserId = t.SubmittedByUserId,
        AssignedToUserId = t.AssignedToUserId,
        ResolutionNotes = t.ResolutionNotes,
        ResolvedAt = t.ResolvedAt,
        CreatedAt = t.CreatedAt,
        UpdatedAt = t.UpdatedAt
    };
}
