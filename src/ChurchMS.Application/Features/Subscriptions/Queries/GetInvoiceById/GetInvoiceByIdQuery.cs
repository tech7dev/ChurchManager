using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Queries.GetInvoiceById;

public record GetInvoiceByIdQuery(Guid Id) : IRequest<ApiResponse<InvoiceDto>>;
