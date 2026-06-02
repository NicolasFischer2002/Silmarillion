using Application.Abstractions.Persistence;
using Application.Memberships.Commands.ActivateMembership;
using Domain.Aggregates.Memberships.Aggregate;
using Domain.Aggregates.Memberships.Constants;
using Domain.Aggregates.Memberships.Errors;
using Moq;
using SharedTests.Assertions;

namespace Tests.Application.Memberships.Commands;

[TestClass]
public class ActivateMembershipCommandHandlerTests
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

        var handler = new ActivateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ActivateMembershipCommand(
            Guid.NewGuid());

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(MembershipErrors.MembershipNotFound());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenMembershipIsAlreadyActive()
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

        var handler = new ActivateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ActivateMembershipCommand(
            membership.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(MembershipErrors.MembershipAlreadyActive());

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

        var handler = new ActivateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ActivateMembershipCommand(
            membership.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(MembershipErrors.CannotActivateRevokedMembership());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldActivateMembership_WhenCommandIsValid()
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

        var handler = new ActivateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ActivateMembershipCommand(membership.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.AreEqual(
            MembershipStatus.Active,
            membership.Status);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldCallSaveChanges_WhenMembershipIsActivated()
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

        var handler = new ActivateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ActivateMembershipCommand(
            membership.Id);

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