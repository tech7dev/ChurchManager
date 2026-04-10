using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.SetMemberCustomFieldValue;

public class SetMemberCustomFieldValueCommandHandler(
    IMemberRepository memberRepository,
    IRepository<CustomField> customFieldRepository,
    IRepository<MemberCustomFieldValue> valueRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<SetMemberCustomFieldValueCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(
        SetMemberCustomFieldValueCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var field = await customFieldRepository.GetByIdAsync(request.CustomFieldId, cancellationToken)
            ?? throw new NotFoundException(nameof(CustomField), request.CustomFieldId);

        // Upsert: find existing value or create new
        var existing = (await valueRepository.FindAsync(
            v => v.MemberId == request.MemberId && v.CustomFieldId == request.CustomFieldId,
            cancellationToken)).FirstOrDefault();

        if (existing is not null)
        {
            existing.Value = request.Value;
            valueRepository.Update(existing);
        }
        else
        {
            var newValue = new MemberCustomFieldValue
            {
                ChurchId = churchId,
                MemberId = request.MemberId,
                CustomFieldId = request.CustomFieldId,
                Value = request.Value
            };
            await valueRepository.AddAsync(newValue, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<bool>.SuccessResult(true, "Custom field value saved.");
    }
}
