using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.RecordFollowUp;

public class RecordFollowUpCommandHandler(
    IRepository<EvangelismContact> contactRepository,
    IRepository<EvangelismFollowUp> followUpRepository,
    IRepository<Member> memberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RecordFollowUpCommand, ApiResponse<EvangelismFollowUpDto>>
{
    public async Task<ApiResponse<EvangelismFollowUpDto>> Handle(
        RecordFollowUpCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<EvangelismFollowUpDto>.FailureResult("Church context required.");

        var contact = await contactRepository.GetByIdAsync(request.ContactId, cancellationToken)
            ?? throw new NotFoundException(nameof(EvangelismContact), request.ContactId);

        // Auto-advance status from New → Contacted on first follow-up
        if (contact.Status == ContactStatus.New)
        {
            contact.Status = ContactStatus.Contacted;
        }

        var followUp = new EvangelismFollowUp
        {
            ChurchId = churchId.Value,
            ContactId = contact.Id,
            Method = request.Method,
            FollowUpDate = request.FollowUpDate,
            Notes = request.Notes,
            ConductedByMemberId = request.ConductedByMemberId
        };

        await followUpRepository.AddAsync(followUp, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? conductedByName = null;
        if (followUp.ConductedByMemberId.HasValue)
        {
            var conductor = await memberRepository.GetByIdAsync(followUp.ConductedByMemberId.Value, cancellationToken);
            conductedByName = conductor is not null ? $"{conductor.FirstName} {conductor.LastName}" : null;
        }

        return ApiResponse<EvangelismFollowUpDto>.SuccessResult(new EvangelismFollowUpDto
        {
            Id = followUp.Id,
            ChurchId = followUp.ChurchId,
            ContactId = followUp.ContactId,
            Method = followUp.Method,
            FollowUpDate = followUp.FollowUpDate,
            Notes = followUp.Notes,
            ConductedByMemberId = followUp.ConductedByMemberId,
            ConductedByName = conductedByName,
            CreatedAt = followUp.CreatedAt
        });
    }
}
