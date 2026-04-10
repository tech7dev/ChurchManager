using ChurchMS.Application.Features.ITManagement.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.ITManagement.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<ApiResponse<UserSummaryDto>>;
