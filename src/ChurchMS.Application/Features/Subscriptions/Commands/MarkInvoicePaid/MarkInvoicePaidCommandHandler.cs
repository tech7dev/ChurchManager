using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.MarkInvoicePaid;

public class MarkInvoicePaidCommandHandler(
    IRepository<Invoice> invoiceRepository,
    IDateTimeService dateTimeService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<MarkInvoicePaidCommand, ApiResponse<InvoiceDto>>
{
    public async Task<ApiResponse<InvoiceDto>> Handle(
        MarkInvoicePaidCommand request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken);
        if (invoice is null)
            return ApiResponse<InvoiceDto>.FailureResult("Invoice not found.");

        if (invoice.Status == InvoiceStatus.Paid)
            return ApiResponse<InvoiceDto>.FailureResult("Invoice is already paid.");

        if (invoice.Status == InvoiceStatus.Cancelled)
            return ApiResponse<InvoiceDto>.FailureResult("Cannot pay a cancelled invoice.");

        invoice.Status = InvoiceStatus.Paid;
        invoice.PaidAt = dateTimeService.UtcNow;
        invoice.PaymentMethod = request.PaymentMethod;
        invoice.PaymentReference = request.PaymentReference;

        invoiceRepository.Update(invoice);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<InvoiceDto>.SuccessResult(MapToDto(invoice));
    }

    internal static InvoiceDto MapToDto(Invoice i) => new()
    {
        Id = i.Id,
        ChurchId = i.ChurchId,
        InvoiceNumber = i.InvoiceNumber,
        Description = i.Description,
        Amount = i.Amount,
        Currency = i.Currency,
        Status = i.Status,
        DueDate = i.DueDate,
        PaidAt = i.PaidAt,
        PaymentMethod = i.PaymentMethod,
        PaymentReference = i.PaymentReference,
        Notes = i.Notes,
        SubscriptionId = i.SubscriptionId,
        CreatedAt = i.CreatedAt
    };
}
