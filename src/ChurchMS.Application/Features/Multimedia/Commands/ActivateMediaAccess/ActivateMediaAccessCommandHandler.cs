using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Commands.ActivateMediaAccess;

public class ActivateMediaAccessCommandHandler(
    IRepository<MediaPurchase> purchaseRepository,
    IRepository<MediaContent> contentRepository,
    IRepository<Member> memberRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ActivateMediaAccessCommand, ApiResponse<MediaPurchaseDto>>
{
    public async Task<ApiResponse<MediaPurchaseDto>> Handle(
        ActivateMediaAccessCommand request, CancellationToken cancellationToken)
    {
        var purchase = await purchaseRepository.GetByIdAsync(request.PurchaseId, cancellationToken)
            ?? throw new NotFoundException(nameof(MediaPurchase), request.PurchaseId);

        if (purchase.Status == MediaPurchaseStatus.Completed)
            return ApiResponse<MediaPurchaseDto>.FailureResult("Purchase is already activated.");

        if (purchase.Status != MediaPurchaseStatus.Pending)
            return ApiResponse<MediaPurchaseDto>.FailureResult("Only pending purchases can be activated.");

        var now = DateTime.UtcNow;
        purchase.Status = MediaPurchaseStatus.Completed;
        purchase.PaidAt ??= now;
        purchase.ActivatedAt = now;
        purchase.ActivatedByMemberId = request.ActivatedByMemberId;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var content = await contentRepository.GetByIdAsync(purchase.ContentId, cancellationToken);
        var member = await memberRepository.GetByIdAsync(purchase.MemberId, cancellationToken);
        var activatedBy = await memberRepository.GetByIdAsync(request.ActivatedByMemberId, cancellationToken);

        return ApiResponse<MediaPurchaseDto>.SuccessResult(new MediaPurchaseDto
        {
            Id = purchase.Id,
            ChurchId = purchase.ChurchId,
            ContentId = purchase.ContentId,
            ContentTitle = content?.Title ?? "",
            MemberId = purchase.MemberId,
            MemberName = member is not null ? $"{member.FirstName} {member.LastName}" : "",
            Amount = purchase.Amount,
            Status = purchase.Status,
            PaymentReference = purchase.PaymentReference,
            PaidAt = purchase.PaidAt,
            ActivatedAt = purchase.ActivatedAt,
            ActivatedByMemberId = purchase.ActivatedByMemberId,
            ActivatedByName = activatedBy is not null ? $"{activatedBy.FirstName} {activatedBy.LastName}" : null,
            CreatedAt = purchase.CreatedAt
        });
    }
}
