using Application.Abstractions.Persistence;
using Application.Memberships.Commands.RevokeMembership;
using Domain.Aggregates.Membership.Aggregate;
using Domain.Aggregates.Membership.Constants;
using Domain.Aggregates.Membership.Errors;
using Domain.Aggregates.Membership.Events;
using Moq;
using SharedTests.Assertions;

namespace Tests.Application.Memberships.Commands;

[TestClass]
public class RevokeMembershipCommandHandlerTests
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

        var handler = new RevokeMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RevokeMembershipCommand(Guid.NewGuid());

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
    public async Task HandleAsync_ShouldReturnFailure_WhenMembershipIsAlreadyRevoked()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var membership = CreateRevokedMembership();

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new RevokeMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RevokeMembershipCommand(membership.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(MembershipErrors.MembershipAlreadyRevoked());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldRevokeMembership_WhenMembershipExists()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var membership = CreateMembership();

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new RevokeMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RevokeMembershipCommand(membership.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.AreEqual(
            MembershipStatus.Revoked,
            membership.Status);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldAddRevokedDomainEvent_WhenMembershipIsRevoked()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var membership = CreateMembership();

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new RevokeMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RevokeMembershipCommand(membership.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.HasCount(
            1,
            membership.DomainEvents);

        Assert.IsInstanceOfType(
            membership.DomainEvents.Single(),
            typeof(MembershipRevokedDomainEvent));
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

    private static Membership CreateRevokedMembership()
    {
        var membership = CreateMembership();

        membership.Revoke();

        return membership;
    }
}