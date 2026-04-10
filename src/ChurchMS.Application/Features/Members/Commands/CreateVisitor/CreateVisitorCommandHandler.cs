using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.CreateVisitor;

public class CreateVisitorCommandHandler(
    IRepository<Visitor> visitorRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateVisitorCommand, ApiResponse<VisitorDto>>
{
    public async Task<ApiResponse<VisitorDto>> Handle(
        CreateVisitorCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var visitor = request.Adapt<Visitor>();
        visitor.ChurchId = churchId;

        await visitorRepository.AddAsync(visitor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<VisitorDto>.SuccessResult(visitor.Adapt<VisitorDto>(), "Visitor recorded successfully.");
    }
}
