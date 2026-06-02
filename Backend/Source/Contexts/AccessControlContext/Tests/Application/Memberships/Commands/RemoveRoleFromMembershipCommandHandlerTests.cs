using Application.Abstractions.Persistence;
using Application.Memberships.Commands.RemoveRoleFromMembership;
using Domain.Aggregates.Memberships.Aggregate;
using Domain.Aggregates.Memberships.Errors;
using Moq;
using SharedTests.Assertions;

namespace Tests.Application.Memberships.Commands;

[TestClass]
public class RemoveRoleFromMembershipCommandHandlerTests
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

        var handler = new RemoveRoleFromMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RemoveRoleFromMembershipCommand(
            Guid.NewGuid(),
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
    public async Task HandleAsync_ShouldReturnFailure_WhenRoleIsNotAssigned()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var membership = CreateMembership();

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new RemoveRoleFromMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RemoveRoleFromMembershipCommand(
            membership.Id,
            Guid.NewGuid());

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(MembershipErrors.RoleNotAssigned());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldRemoveRole_WhenCommandIsValid()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var membership = CreateMembership();

        var roleId = Guid.NewGuid();

        membership.AssignRole(roleId);

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new RemoveRoleFromMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RemoveRoleFromMembershipCommand(
            membership.Id,
            roleId);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.IsFalse(membership.RoleIds.Contains(roleId));

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldPersistMembership_WhenCommandIsValid()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var membership = CreateMembership();

        var roleId = Guid.NewGuid();

        membership.AssignRole(roleId);

        membershipRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new RemoveRoleFromMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RemoveRoleFromMembershipCommand(
            membership.Id,
            roleId);

        await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        Assert.IsFalse(membership.RoleIds.Contains(roleId));

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