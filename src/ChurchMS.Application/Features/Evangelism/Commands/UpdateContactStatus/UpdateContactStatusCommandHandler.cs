using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.UpdateContactStatus;

public class UpdateContactStatusCommandHandler(
    IRepository<EvangelismContact> contactRepository,
    IRepository<EvangelismFollowUp> followUpRepository,
    IRepository<Member> memberRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateContactStatusCommand, ApiResponse<EvangelismContactDto>>
{
    public async Task<ApiResponse<EvangelismContactDto>> Handle(
        UpdateContactStatusCommand request, CancellationToken cancellationToken)
    {
        var contact = await contactRepository.GetByIdAsync(request.ContactId, cancellationToken)
            ?? throw new NotFoundException(nameof(EvangelismContact), request.ContactId);

        contact.Status = request.Status;

        if (request.Status == ContactStatus.Converted)
        {
            contact.ConvertedAt ??= DateTime.UtcNow;
            contact.ConvertedMemberId = request.ConvertedMemberId ?? contact.ConvertedMemberId;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? assignedToName = null;
        if (contact.AssignedToMemberId.HasValue)
        {
            var assignedTo = await memberRepository.GetByIdAsync(contact.AssignedToMemberId.Value, cancellationToken);
            assignedToName = assignedTo is not null ? $"{assignedTo.FirstName} {assignedTo.LastName}" : null;
        }

        var followUpCount = await followUpRepository.CountAsync(f => f.ContactId == contact.Id, cancellationToken);

        return ApiResponse<EvangelismContactDto>.SuccessResult(new EvangelismContactDto
        {
            Id = contact.Id,
            ChurchId = contact.ChurchId,
            CampaignId = contact.CampaignId,
            TeamId = contact.TeamId,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Phone = contact.Phone,
            Email = contact.Email,
            Address = contact.Address,
            Status = contact.Status,
            AssignedToMemberId = contact.AssignedToMemberId,
            AssignedToName = assignedToName,
            Notes = contact.Notes,
            ConvertedAt = contact.ConvertedAt,
            ConvertedMemberId = contact.ConvertedMemberId,
            FollowUpCount = followUpCount,
            CreatedAt = contact.CreatedAt
        });
    }
}
