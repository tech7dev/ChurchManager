using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Members.Queries.GetCustomFields;

public class GetCustomFieldsQueryHandler(IRepository<CustomField> customFieldRepository)
    : IRequestHandler<GetCustomFieldsQuery, ApiResponse<List<CustomFieldDto>>>
{
    public async Task<ApiResponse<List<CustomFieldDto>>> Handle(
        GetCustomFieldsQuery request,
        CancellationToken cancellationToken)
    {
        var fields = await customFieldRepository.GetAllAsync(cancellationToken);
        var dtos = fields.OrderBy(f => f.DisplayOrder).Adapt<List<CustomFieldDto>>();
        return ApiResponse<List<CustomFieldDto>>.SuccessResult(dtos);
    }
}
