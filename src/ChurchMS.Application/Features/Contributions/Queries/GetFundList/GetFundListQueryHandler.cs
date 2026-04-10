using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetFundList;

public class GetFundListQueryHandler(
    IRepository<Fund> fundRepository,
    ITenantService tenantService)
    : IRequestHandler<GetFundListQuery, ApiResponse<IList<FundDto>>>
{
    public async Task<ApiResponse<IList<FundDto>>> Handle(
        GetFundListQuery request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();

        var funds = await fundRepository.FindAsync(
            f => (!request.ActiveOnly || f.IsActive)
                 && (!churchId.HasValue || f.ChurchId == churchId.Value),
            cancellationToken);

        return ApiResponse<IList<FundDto>>.SuccessResult(
            funds.Adapt<IList<FundDto>>());
    }
}
