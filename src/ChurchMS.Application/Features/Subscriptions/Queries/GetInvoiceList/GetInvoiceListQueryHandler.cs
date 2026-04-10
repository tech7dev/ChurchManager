using ChurchMS.Application.Features.Subscriptions.Commands.MarkInvoicePaid;
using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Queries.GetInvoiceList;

public class GetInvoiceListQueryHandler(IRepository<Invoice> invoiceRepository)
    : IRequestHandler<GetInvoiceListQuery, ApiResponse<PagedResult<InvoiceDto>>>
{
    public async Task<ApiResponse<PagedResult<InvoiceDto>>> Handle(
        GetInvoiceListQuery request, CancellationToken cancellationToken)
    {
        var all = await invoiceRepository.FindAsync(
            i => !request.Status.HasValue || i.Status == request.Status.Value,
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(i => i.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(MarkInvoicePaidCommandHandler.MapToDto)
            .ToList();

        return ApiResponse<PagedResult<InvoiceDto>>.SuccessResult(new PagedResult<InvoiceDto>
        {
            Items = paged,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
