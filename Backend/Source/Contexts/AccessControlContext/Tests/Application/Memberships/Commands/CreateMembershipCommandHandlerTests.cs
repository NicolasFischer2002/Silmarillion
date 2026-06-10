using Application.Abstractions.Persistence;
using Application.Memberships.Commands.CreateMembership;
using Domain.Aggregates.Memberships.Aggregate;
using Domain.Aggregates.Memberships.Constants;
using Domain.Aggregates.Memberships.Errors;
using Moq;
using SharedTests.Assertions;

namespace Tests.Application.Memberships.Commands;

[TestClass]
public class CreateMembershipCommandHandlerTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public async Task HandleAsync_ShouldReturnSuccess_WhenCommandIsValid()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var handler = new CreateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateMembershipCommand(
            Guid.NewGuid(),
            Guid.NewGuid());

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.IsNotNull(result.Value);

        Assert.AreEqual(
            command.UserId,
            result.Value.UserId);

        Assert.AreEqual(
            command.OrganizationId,
            result.Value.OrganizationId);

        membershipRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Membership>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenUserIdIsEmpty()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var handler = new CreateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateMembershipCommand(
            Guid.Empty,
            Guid.NewGuid());

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(MembershipErrors.UserIdRequired());

        membershipRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Membership>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenOrganizationIdIsEmpty()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var handler = new CreateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateMembershipCommand(
            Guid.NewGuid(),
            Guid.Empty);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(MembershipErrors.OrganizationIdRequired());

        membershipRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Membership>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldPersistMembership_WhenCommandIsValid()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        Membership? capturedMembership = null;

        membershipRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Membership>(),
                It.IsAny<CancellationToken>()))
            .Callback<Membership, CancellationToken>((membership, _) =>
            {
                capturedMembership = membership;
            });

        var handler = new CreateMembershipCommandHandler(
            membershipRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateMembershipCommand(
            Guid.NewGuid(),
            Guid.NewGuid());

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.IsNotNull(capturedMembership);

        Assert.AreEqual(
            command.UserId,
            capturedMembership.UserId);

        Assert.AreEqual(
            command.OrganizationId,
            capturedMembership.OrganizationId);

        Assert.AreEqual(
            MembershipStatus.Pending,
            capturedMembership.Status);
    }
}