using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.UpdateContactStatus;

public record UpdateContactStatusCommand(
    Guid ContactId,
    ContactStatus Status,
    Guid? ConvertedMemberId
) : IRequest<ApiResponse<EvangelismContactDto>>;
