using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Queries.GetFamilyById;

public record GetFamilyByIdQuery(Guid Id) : IRequest<ApiResponse<FamilyDto>>;
