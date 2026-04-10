using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Commands.ActivateMediaAccess;

/// <summary>
/// Staff manually activates a pending (cash) purchase.
/// </summary>
public record ActivateMediaAccessCommand(
    Guid PurchaseId,
    Guid ActivatedByMemberId
) : IRequest<ApiResponse<MediaPurchaseDto>>;
