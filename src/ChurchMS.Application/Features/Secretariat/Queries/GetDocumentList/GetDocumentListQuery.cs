using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Queries.GetDocumentList;

public record GetDocumentListQuery(
    DocumentType? Type = null,
    Guid? MemberId = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<DocumentDto>>>;
