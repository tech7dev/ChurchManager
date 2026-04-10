using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Queries.GetFollowUpList;

public class GetFollowUpListQueryHandler(
    IRepository<EvangelismFollowUp> followUpRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetFollowUpListQuery, ApiResponse<PagedResult<EvangelismFollowUpDto>>>
{
    public async Task<ApiResponse<PagedResult<EvangelismFollowUpDto>>> Handle(
        GetFollowUpListQuery request, CancellationToken cancellationToken)
    {
        var all = await followUpRepository.FindAsync(
            f => f.ContactId == request.ContactId,
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(f => f.FollowUpDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<EvangelismFollowUpDto>();
        foreach (var followUp in paged)
        {
            string? conductedByName = null;
            if (followUp.ConductedByMemberId.HasValue)
            {
                var conductor = await memberRepository.GetByIdAsync(followUp.ConductedByMemberId.Value, cancellationToken);
                conductedByName = conductor is not null ? $"{conductor.FirstName} {conductor.LastName}" : null;
            }

            dtos.Add(new EvangelismFollowUpDto
            {
                Id = followUp.Id,
                ChurchId = followUp.ChurchId,
                ContactId = followUp.ContactId,
                Method = followUp.Method,
                FollowUpDate = followUp.FollowUpDate,
                Notes = followUp.Notes,
                ConductedByMemberId = followUp.ConductedByMemberId,
                ConductedByName = conductedByName,
                CreatedAt = followUp.CreatedAt
            });
        }

        return ApiResponse<PagedResult<EvangelismFollowUpDto>>.SuccessResult(new PagedResult<EvangelismFollowUpDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
