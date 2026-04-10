using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.AddContact;

public record AddContactCommand(
    Guid CampaignId,
    Guid? TeamId,
    string FirstName,
    string LastName,
    string? Phone,
    string? Email,
    string? Address,
    Guid? AssignedToMemberId,
    string? Notes
) : IRequest<ApiResponse<EvangelismContactDto>>;
