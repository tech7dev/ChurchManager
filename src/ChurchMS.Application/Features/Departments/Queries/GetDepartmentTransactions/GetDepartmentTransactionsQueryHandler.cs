using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Queries.GetDepartmentTransactions;

public class GetDepartmentTransactionsQueryHandler(
    IRepository<DepartmentTransaction> transactionRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetDepartmentTransactionsQuery, ApiResponse<PagedResult<DepartmentTransactionDto>>>
{
    public async Task<ApiResponse<PagedResult<DepartmentTransactionDto>>> Handle(
        GetDepartmentTransactionsQuery request, CancellationToken cancellationToken)
    {
        var all = await transactionRepository.FindAsync(
            t => t.DepartmentId == request.DepartmentId
              && (!request.Type.HasValue || t.Type == request.Type.Value)
              && (!request.FromDate.HasValue || t.Date >= request.FromDate.Value)
              && (!request.ToDate.HasValue || t.Date <= request.ToDate.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(t => t.Date)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<DepartmentTransactionDto>();
        foreach (var t in paged)
        {
            string? recorderName = null;
            if (t.RecordedByMemberId.HasValue)
            {
                var recorder = await memberRepository.GetByIdAsync(t.RecordedByMemberId.Value, cancellationToken);
                recorderName = recorder is not null ? $"{recorder.FirstName} {recorder.LastName}" : null;
            }

            dtos.Add(new DepartmentTransactionDto
            {
                Id = t.Id,
                DepartmentId = t.DepartmentId,
                Type = t.Type,
                Amount = t.Amount,
                Description = t.Description,
                Date = t.Date,
                ReferenceNumber = t.ReferenceNumber,
                RecordedByMemberId = t.RecordedByMemberId,
                RecordedByName = recorderName,
                CreatedAt = t.CreatedAt
            });
        }

        return ApiResponse<PagedResult<DepartmentTransactionDto>>.SuccessResult(new PagedResult<DepartmentTransactionDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
