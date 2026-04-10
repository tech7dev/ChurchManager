using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Queries.GetSmsCreditBalance;

public class GetSmsCreditBalanceQueryHandler(
    IRepository<SmsCredit> smsCreditRepository,
    ITenantService tenantService)
    : IRequestHandler<GetSmsCreditBalanceQuery, ApiResponse<SmsCreditDto>>
{
    public async Task<ApiResponse<SmsCreditDto>> Handle(
        GetSmsCreditBalanceQuery request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<SmsCreditDto>.FailureResult("Church context required.");

        var wallets = await smsCreditRepository.FindAsync(
            w => w.ChurchId == churchId.Value, cancellationToken);

        if (wallets.Count == 0)
            return ApiResponse<SmsCreditDto>.SuccessResult(new SmsCreditDto
            {
                ChurchId = churchId.Value,
                Balance = 0,
                TotalPurchased = 0,
                TotalConsumed = 0
            });

        var wallet = wallets[0];
        return ApiResponse<SmsCreditDto>.SuccessResult(new SmsCreditDto
        {
            Id = wallet.Id,
            ChurchId = wallet.ChurchId,
            Balance = wallet.Balance,
            TotalPurchased = wallet.TotalPurchased,
            TotalConsumed = wallet.TotalConsumed
        });
    }
}
