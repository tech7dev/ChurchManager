using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Members.Commands.CreateMember;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace ChurchMS.UnitTests.Features.Members;

public class CreateMemberCommandHandlerTests
{
    private readonly IMemberRepository _memberRepo = Substitute.For<IMemberRepository>();
    private readonly IFamilyRepository _familyRepo = Substitute.For<IFamilyRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ITenantService _tenantService = Substitute.For<ITenantService>();

    private CreateMemberCommandHandler CreateHandler() =>
        new(_memberRepo, _familyRepo, _unitOfWork, _tenantService);

    [Fact]
    public async Task Handle_WithValidData_ReturnsMemberDto()
    {
        // Arrange
        var churchId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        _tenantService.GetCurrentChurchId().Returns(churchId);
        _memberRepo.GenerateNextMembershipNumberAsync(churchId, Arg.Any<CancellationToken>())
            .Returns("MBR-0001");

        var savedMember = new Member
        {
            Id = memberId,
            ChurchId = churchId,
            FirstName = "Alice",
            LastName = "Smith",
            MembershipNumber = "MBR-0001"
        };
        _memberRepo.GetWithDetailsAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(savedMember);

        var command = new CreateMemberCommand
        {
            FirstName = "Alice",
            LastName = "Smith",
            Gender = Gender.Female,
            JoinDate = DateOnly.FromDateTime(DateTime.Today)
        };

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.FirstName.Should().Be("Alice");
        result.Data.MembershipNumber.Should().Be("MBR-0001");

        await _memberRepo.Received(1).AddAsync(
            Arg.Is<Member>(m => m.ChurchId == churchId && m.FirstName == "Alice"),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithoutChurchContext_ThrowsForbiddenException()
    {
        // Arrange
        _tenantService.GetCurrentChurchId().Returns((Guid?)null);

        var command = new CreateMemberCommand { FirstName = "Bob", LastName = "Jones" };
        var handler = CreateHandler();

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>();
        await _memberRepo.DidNotReceive().AddAsync(Arg.Any<Member>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNonExistentFamily_ThrowsNotFoundException()
    {
        // Arrange
        var churchId = Guid.NewGuid();
        var familyId = Guid.NewGuid();
        _tenantService.GetCurrentChurchId().Returns(churchId);
        _familyRepo.GetByIdAsync(familyId, Arg.Any<CancellationToken>()).Returns((Family?)null);

        var command = new CreateMemberCommand
        {
            FirstName = "Carol",
            LastName = "Brown",
            FamilyId = familyId
        };

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
