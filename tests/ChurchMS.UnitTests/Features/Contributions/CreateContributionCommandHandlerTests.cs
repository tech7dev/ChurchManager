using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Contributions.Commands.CreateContribution;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace ChurchMS.UnitTests.Features.Contributions;

public class CreateContributionCommandHandlerTests
{
    private readonly IRepository<Contribution> _contributionRepo = Substitute.For<IRepository<Contribution>>();
    private readonly IRepository<Fund> _fundRepo = Substitute.For<IRepository<Fund>>();
    private readonly IRepository<ContributionCampaign> _campaignRepo = Substitute.For<IRepository<ContributionCampaign>>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ITenantService _tenantService = Substitute.For<ITenantService>();

    private CreateContributionCommandHandler CreateHandler() =>
        new(_contributionRepo, _fundRepo, _campaignRepo, _unitOfWork, _tenantService);

    [Fact]
    public async Task Handle_WithValidData_ReturnsContributionDto()
    {
        // Arrange
        var churchId = Guid.NewGuid();
        var fundId = Guid.NewGuid();

        var fund = new Fund { Id = fundId, ChurchId = churchId, Name = "General Fund" };
        _tenantService.GetCurrentChurchId().Returns(churchId);
        _fundRepo.GetByIdAsync(fundId, Arg.Any<CancellationToken>()).Returns(fund);

        // Mapster flattens Fund.Name → FundName; inject Fund navigation so it doesn't NullRef
        _contributionRepo.When(r => r.AddAsync(Arg.Any<Contribution>(), Arg.Any<CancellationToken>()))
            .Do(call => call.ArgAt<Contribution>(0).Fund = fund);

        var command = new CreateContributionCommand(
            Amount: 100.00m,
            Currency: "USD",
            ContributionDate: DateOnly.FromDateTime(DateTime.Today),
            Type: ContributionType.Cash,
            Notes: null,
            CheckNumber: null,
            TransactionReference: null,
            MemberId: null,              // null to avoid Mapster flattening Member navigation
            AnonymousDonorName: "Anonymous Donor",
            FundId: fundId,
            CampaignId: null,
            IsRecurring: false,
            RecurrenceFrequency: null,
            RecurrenceEndDate: null);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Amount.Should().Be(100.00m);
        result.Data.FundName.Should().Be("General Fund");
        result.Data.ReferenceNumber.Should().StartWith("CTB-");

        await _contributionRepo.Received(1).AddAsync(
            Arg.Is<Contribution>(c =>
                c.ChurchId == churchId &&
                c.Amount == 100.00m &&
                c.FundId == fundId),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNonExistentFund_ThrowsNotFoundException()
    {
        // Arrange
        var fundId = Guid.NewGuid();
        _tenantService.GetCurrentChurchId().Returns(Guid.NewGuid());
        _fundRepo.GetByIdAsync(fundId, Arg.Any<CancellationToken>()).Returns((Fund?)null);

        var command = new CreateContributionCommand(
            Amount: 50m,
            Currency: "USD",
            ContributionDate: DateOnly.FromDateTime(DateTime.Today),
            Type: ContributionType.Cash,
            Notes: null,
            CheckNumber: null,
            TransactionReference: null,
            MemberId: null,
            AnonymousDonorName: "Anonymous",
            FundId: fundId,
            CampaignId: null,
            IsRecurring: false,
            RecurrenceFrequency: null,
            RecurrenceEndDate: null);

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WithoutChurchContext_ThrowsForbiddenException()
    {
        // Arrange
        _tenantService.GetCurrentChurchId().Returns((Guid?)null);

        var command = new CreateContributionCommand(
            Amount: 25m,
            Currency: "USD",
            ContributionDate: DateOnly.FromDateTime(DateTime.Today),
            Type: ContributionType.Cash,
            Notes: null,
            CheckNumber: null,
            TransactionReference: null,
            MemberId: null,
            AnonymousDonorName: null,
            FundId: Guid.NewGuid(),
            CampaignId: null,
            IsRecurring: false,
            RecurrenceFrequency: null,
            RecurrenceEndDate: null);

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>();
    }
}
