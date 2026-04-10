using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Commands.CreateFund;

public class CreateFundCommandHandler(
    IRepository<Fund> fundRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateFundCommand, ApiResponse<FundDto>>
{
    public async Task<ApiResponse<FundDto>> Handle(
        CreateFundCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        // If this is being set as default, unset others
        if (request.IsDefault)
        {
            var existingFunds = await fundRepository.FindAsync(
                f => f.ChurchId == churchId && f.IsDefault, cancellationToken);
            foreach (var f in existingFunds)
            {
                f.IsDefault = false;
                fundRepository.Update(f);
            }
        }

        var fund = new Fund
        {
            ChurchId = churchId,
            Name = request.Name,
            Description = request.Description,
            IsDefault = request.IsDefault,
            IsActive = true
        };

        await fundRepository.AddAsync(fund, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<FundDto>.SuccessResult(fund.Adapt<FundDto>(), "Fund created successfully.");
    }
}
