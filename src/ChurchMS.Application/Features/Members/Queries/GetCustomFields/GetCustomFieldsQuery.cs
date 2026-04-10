using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Queries.GetCustomFields;

public record GetCustomFieldsQuery : IRequest<ApiResponse<List<CustomFieldDto>>>;
