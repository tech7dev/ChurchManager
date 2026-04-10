using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Commands.CreateFund;

public record CreateFundCommand(
    string Name,
    string? Description,
    bool IsDefault) : IRequest<ApiResponse<FundDto>>;
