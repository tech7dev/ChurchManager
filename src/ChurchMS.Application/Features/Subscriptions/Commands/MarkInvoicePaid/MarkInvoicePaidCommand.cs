using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.MarkInvoicePaid;

public record MarkInvoicePaidCommand(
    Guid InvoiceId,
    PaymentMethod PaymentMethod,
    string? PaymentReference
) : IRequest<ApiResponse<InvoiceDto>>;
