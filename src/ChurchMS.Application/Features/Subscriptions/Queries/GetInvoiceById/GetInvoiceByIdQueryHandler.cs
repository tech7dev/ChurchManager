using ChurchMS.Application.Features.Subscriptions.Commands.MarkInvoicePaid;
using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Queries.GetInvoiceById;

public class GetInvoiceByIdQueryHandler(IRepository<Invoice> invoiceRepository)
    : IRequestHandler<GetInvoiceByIdQuery, ApiResponse<InvoiceDto>>
{
    public async Task<ApiResponse<InvoiceDto>> Handle(
        GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (invoice is null)
            return ApiResponse<InvoiceDto>.FailureResult("Invoice not found.");

        return ApiResponse<InvoiceDto>.SuccessResult(MarkInvoicePaidCommandHandler.MapToDto(invoice));
    }
}
