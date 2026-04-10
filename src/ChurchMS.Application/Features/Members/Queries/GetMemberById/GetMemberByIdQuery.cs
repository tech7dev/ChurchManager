using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Queries.GetMemberById;

public record GetMemberByIdQuery(Guid Id) : IRequest<ApiResponse<MemberDto>>;
