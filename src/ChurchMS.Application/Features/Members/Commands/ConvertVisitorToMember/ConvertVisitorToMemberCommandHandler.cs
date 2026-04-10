using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.ConvertVisitorToMember;

public class ConvertVisitorToMemberCommandHandler(
    IRepository<Visitor> visitorRepository,
    IMemberRepository memberRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ConvertVisitorToMemberCommand, ApiResponse<MemberDto>>
{
    public async Task<ApiResponse<MemberDto>> Handle(
        ConvertVisitorToMemberCommand request,
        CancellationToken cancellationToken)
    {
        var visitor = await visitorRepository.GetByIdAsync(request.VisitorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Visitor), request.VisitorId);

        if (visitor.ConvertedToMemberId.HasValue)
            throw new BadRequestException("This visitor has already been converted to a member.");

        var member = new Member
        {
            ChurchId = visitor.ChurchId,
            FirstName = visitor.FirstName,
            LastName = visitor.LastName,
            Phone = visitor.Phone,
            Email = visitor.Email,
            Address = visitor.Address,
            Gender = visitor.Gender,
            JoinDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = MemberStatus.Active,
            QrCodeValue = $"MEMBER:{visitor.ChurchId}:{Guid.NewGuid()}",
            MembershipNumber = await memberRepository.GenerateNextMembershipNumberAsync(visitor.ChurchId, cancellationToken)
        };

        await memberRepository.AddAsync(member, cancellationToken);

        visitor.ConvertedToMemberId = member.Id;
        visitor.ConvertedAt = DateTime.UtcNow;
        visitor.Status = VisitorStatus.Converted;
        visitorRepository.Update(visitor);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<MemberDto>.SuccessResult(member.Adapt<MemberDto>(), "Visitor converted to member successfully.");
    }
}
