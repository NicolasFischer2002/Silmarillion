using Application.Abstractions.Persistence;
using Application.Memberships.Commands.DeactivateMembership;
using Domain.Aggregates.Membership.Aggregate;
using Domain.Aggregates.Membership.Constants;
using Domain.Aggregates.Membership.Errors;
using Moq;
using SharedTests.Assertions;

namespace Tests.Application.Memberships.Commands;

[TestClass]
public class DeactivateMembershipCommandHandlerTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenMembershipDoesNotExist()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Membership?)null);

        var handler = new DeactivateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new DeactivateMembershipCommand(Guid.NewGuid());

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(
            MembershipErrors.MembershipNotFound());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenMembershipIsAlreadyInactive()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var membership = CreateMembership();

        membership.Deactivate();

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new DeactivateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new DeactivateMembershipCommand(membership.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(MembershipErrors.MembershipAlreadyInactive());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenMembershipIsRevoked()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var membership = CreateMembership();

        membership.Revoke();

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new DeactivateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new DeactivateMembershipCommand(membership.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(MembershipErrors.CannotDeactivateRevokedMembership());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldDeactivateMembership_WhenCommandIsValid()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var membership = CreateMembership();

        membership.Activate();

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new DeactivateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new DeactivateMembershipCommand(membership.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.AreEqual(
            MembershipStatus.Inactive,
            membership.Status);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldCallSaveChanges_WhenMembershipIsDeactivated()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var membership = CreateMembership();

        membership.Activate();

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new DeactivateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new DeactivateMembershipCommand(membership.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static Membership CreateMembership()
    {
        var result = Membership.CreatePending(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid());

        result.ShouldSucceed();

        return result.Value;
    }
}