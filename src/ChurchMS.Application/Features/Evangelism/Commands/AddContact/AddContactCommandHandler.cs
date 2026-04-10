using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.AddContact;

public class AddContactCommandHandler(
    IRepository<EvangelismCampaign> campaignRepository,
    IRepository<EvangelismContact> contactRepository,
    IRepository<Member> memberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddContactCommand, ApiResponse<EvangelismContactDto>>
{
    public async Task<ApiResponse<EvangelismContactDto>> Handle(
        AddContactCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<EvangelismContactDto>.FailureResult("Church context required.");

        _ = await campaignRepository.GetByIdAsync(request.CampaignId, cancellationToken)
            ?? throw new NotFoundException(nameof(EvangelismCampaign), request.CampaignId);

        var contact = new EvangelismContact
        {
            ChurchId = churchId.Value,
            CampaignId = request.CampaignId,
            TeamId = request.TeamId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address,
            AssignedToMemberId = request.AssignedToMemberId,
            Notes = request.Notes,
            Status = ContactStatus.New
        };

        await contactRepository.AddAsync(contact, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string? assignedToName = null;
        if (contact.AssignedToMemberId.HasValue)
        {
            var assignedTo = await memberRepository.GetByIdAsync(contact.AssignedToMemberId.Value, cancellationToken);
            assignedToName = assignedTo is not null ? $"{assignedTo.FirstName} {assignedTo.LastName}" : null;
        }

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
            FollowUpCount = 0,
            CreatedAt = contact.CreatedAt
        });
    }
}
