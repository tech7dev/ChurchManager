using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.GrowthSchool.Commands.EnrollMember;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace ChurchMS.UnitTests.Features.GrowthSchool;

public class EnrollMemberCommandHandlerTests
{
    private readonly IRepository<GrowthSchoolEnrollment> _enrollmentRepo = Substitute.For<IRepository<GrowthSchoolEnrollment>>();
    private readonly IRepository<GrowthSchoolCourse> _courseRepo = Substitute.For<IRepository<GrowthSchoolCourse>>();
    private readonly IRepository<Member> _memberRepo = Substitute.For<IRepository<Member>>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ITenantService _tenantService = Substitute.For<ITenantService>();

    private EnrollMemberCommandHandler CreateHandler() =>
        new(_enrollmentRepo, _courseRepo, _memberRepo, _unitOfWork, _tenantService);

    [Fact]
    public async Task Handle_WithValidData_ReturnsEnrollmentDto()
    {
        // Arrange
        var churchId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        _tenantService.GetCurrentChurchId().Returns(churchId);

        var course = new GrowthSchoolCourse { Id = courseId, ChurchId = churchId, Name = "New Believers" };
        var member = new Member { Id = memberId, ChurchId = churchId, FirstName = "John", LastName = "Doe" };

        _courseRepo.GetByIdAsync(courseId, Arg.Any<CancellationToken>()).Returns(course);
        _memberRepo.GetByIdAsync(memberId, Arg.Any<CancellationToken>()).Returns(member);
        _enrollmentRepo.FindAsync(Arg.Any<System.Linq.Expressions.Expression<Func<GrowthSchoolEnrollment, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(new List<GrowthSchoolEnrollment>());

        var command = new EnrollMemberCommand(courseId, memberId, DateOnly.FromDateTime(DateTime.Today), null);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.CourseName.Should().Be("New Believers");
        result.Data.MemberName.Should().Be("John Doe");
        result.Data.Status.Should().Be(EnrollmentStatus.Active);

        await _enrollmentRepo.Received(1).AddAsync(Arg.Any<GrowthSchoolEnrollment>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAlreadyEnrolled_ThrowsBadRequestException()
    {
        // Arrange
        var churchId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        _tenantService.GetCurrentChurchId().Returns(churchId);
        _courseRepo.GetByIdAsync(courseId, Arg.Any<CancellationToken>())
            .Returns(new GrowthSchoolCourse { Id = courseId, ChurchId = churchId, Name = "Course" });
        _memberRepo.GetByIdAsync(memberId, Arg.Any<CancellationToken>())
            .Returns(new Member { Id = memberId, ChurchId = churchId, FirstName = "Jane", LastName = "Doe" });

        // Existing active enrollment
        _enrollmentRepo.FindAsync(Arg.Any<System.Linq.Expressions.Expression<Func<GrowthSchoolEnrollment, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(new List<GrowthSchoolEnrollment> { new() { CourseId = courseId, MemberId = memberId, Status = EnrollmentStatus.Active } });

        var command = new EnrollMemberCommand(courseId, memberId, DateOnly.FromDateTime(DateTime.Today), null);
        var handler = CreateHandler();

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("*already enrolled*");
    }

    [Fact]
    public async Task Handle_WithNonExistentCourse_ThrowsNotFoundException()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        _tenantService.GetCurrentChurchId().Returns(Guid.NewGuid());
        _courseRepo.GetByIdAsync(courseId, Arg.Any<CancellationToken>()).Returns((GrowthSchoolCourse?)null);

        var command = new EnrollMemberCommand(courseId, Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), null);
        var handler = CreateHandler();

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
