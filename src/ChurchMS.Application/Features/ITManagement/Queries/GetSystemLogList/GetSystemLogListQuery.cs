using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetSystemLogList;

public record GetSystemLogListQuery(
    string? Action = null,
    string? EntityType = null,
    string? Level = null,
    DateTime? From = null,
    DateTime? To = null,
    int Page = 1,
    int PageSize = 50
) : IRequest<ApiResponse<PagedResult<SystemLogDto>>>;
