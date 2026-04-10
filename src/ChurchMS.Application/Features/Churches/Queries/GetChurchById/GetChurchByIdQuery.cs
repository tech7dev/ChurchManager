using ChurchMS.Application.Features.Churches.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Churches.Queries.GetChurchById;

public record GetChurchByIdQuery(Guid Id) : IRequest<ApiResponse<ChurchDto>>;
