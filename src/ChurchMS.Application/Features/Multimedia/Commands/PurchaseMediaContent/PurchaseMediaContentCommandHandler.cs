using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Commands.PurchaseMediaContent;

public class PurchaseMediaContentCommandHandler(
    IRepository<MediaContent> contentRepository,
    IRepository<MediaPurchase> purchaseRepository,
    IRepository<Member> memberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PurchaseMediaContentCommand, ApiResponse<MediaPurchaseDto>>
{
    public async Task<ApiResponse<MediaPurchaseDto>> Handle(
        PurchaseMediaContentCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<MediaPurchaseDto>.FailureResult("Church context required.");

        var content = await contentRepository.GetByIdAsync(request.ContentId, cancellationToken)
            ?? throw new NotFoundException(nameof(MediaContent), request.ContentId);

        if (content.Status != MediaContentStatus.Published)
            return ApiResponse<MediaPurchaseDto>.FailureResult("Content is not available for purchase.");

        if (content.AccessType != MediaAccessType.Paid)
            return ApiResponse<MediaPurchaseDto>.FailureResult("This content does not require a purchase.");

        // Prevent duplicate purchases
        var existing = await purchaseRepository.FindAsync(
            p => p.ContentId == content.Id
              && p.MemberId == request.MemberId
              && p.Status == MediaPurchaseStatus.Completed,
            cancellationToken);

        if (existing.Count > 0)
            return ApiResponse<MediaPurchaseDto>.FailureResult("Member has already purchased this content.");

        var now = DateTime.UtcNow;
        var isOnlinePayment = !string.IsNullOrWhiteSpace(request.PaymentReference);

        var purchase = new MediaPurchase
        {
            ChurchId = churchId.Value,
            ContentId = content.Id,
            MemberId = request.MemberId,
            Amount = content.Price ?? 0,
            PaymentReference = request.PaymentReference,
            // Online payment with a reference is auto-completed; cash requires staff activation
            Status = isOnlinePayment ? MediaPurchaseStatus.Completed : MediaPurchaseStatus.Pending,
            PaidAt = isOnlinePayment ? now : null
        };

        await purchaseRepository.AddAsync(purchase, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var member = await memberRepository.GetByIdAsync(purchase.MemberId, cancellationToken);
        var memberName = member is not null ? $"{member.FirstName} {member.LastName}" : "";

        return ApiResponse<MediaPurchaseDto>.SuccessResult(new MediaPurchaseDto
        {
            Id = purchase.Id,
            ChurchId = purchase.ChurchId,
            ContentId = purchase.ContentId,
            ContentTitle = content.Title,
            MemberId = purchase.MemberId,
            MemberName = memberName,
            Amount = purchase.Amount,
            Status = purchase.Status,
            PaymentReference = purchase.PaymentReference,
            PaidAt = purchase.PaidAt,
            CreatedAt = purchase.CreatedAt
        });
    }
}
