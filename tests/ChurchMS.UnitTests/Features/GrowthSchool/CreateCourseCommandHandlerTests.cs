using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.GrowthSchool.Commands.CreateCourse;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace ChurchMS.UnitTests.Features.GrowthSchool;

public class CreateCourseCommandHandlerTests
{
    private readonly IRepository<GrowthSchoolCourse> _courseRepo = Substitute.For<IRepository<GrowthSchoolCourse>>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ITenantService _tenantService = Substitute.For<ITenantService>();

    private CreateCourseCommandHandler CreateHandler() =>
        new(_courseRepo, _unitOfWork, _tenantService);

    [Fact]
    public async Task Handle_WithValidData_ReturnsCourseDto()
    {
        // Arrange
        var churchId = Guid.NewGuid();
        _tenantService.GetCurrentChurchId().Returns(churchId);

        var command = new CreateCourseCommand(
            Name: "New Believers",
            Description: "Foundational course for new believers",
            Level: GrowthSchoolLevel.Foundational,
            InstructorId: null,
            DurationWeeks: 8,
            MaxCapacity: 30);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be("New Believers");
        result.Data.Level.Should().Be(GrowthSchoolLevel.Foundational);
        result.Data.ChurchId.Should().Be(churchId);
        result.Data.IsActive.Should().BeTrue();

        await _courseRepo.Received(1).AddAsync(
            Arg.Is<GrowthSchoolCourse>(c =>
                c.Name == "New Believers" &&
                c.ChurchId == churchId &&
                c.IsActive),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithoutChurchContext_ThrowsForbiddenException()
    {
        // Arrange
        _tenantService.GetCurrentChurchId().Returns((Guid?)null);

        var command = new CreateCourseCommand(
            Name: "Leadership 101",
            Description: null,
            Level: GrowthSchoolLevel.Leadership,
            InstructorId: null,
            DurationWeeks: null,
            MaxCapacity: null);

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>();
        await _courseRepo.DidNotReceive().AddAsync(Arg.Any<GrowthSchoolCourse>(), Arg.Any<CancellationToken>());
    }
}
