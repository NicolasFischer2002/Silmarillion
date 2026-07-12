using Application.Abstractions.Persistence;
using Application.Roles.Commands.ActivateRole;
using Domain.Aggregates.Role.Aggregate;
using Domain.Aggregates.Role.Errors;
using Domain.Aggregates.Role.ValueObjects;
using Domain.Aggregates.Roles.Constants;
using Moq;
using SharedTests.Assertions;

namespace Tests.Application.Roles.Commands;

[TestClass]
public class ActivateRoleCommandHandlerTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenRoleDoesNotExist()
    {
        var roleRepositoryMock = new Mock<IRoleRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Role?)null);

        var handler = new ActivateRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ActivateRoleCommand(Guid.NewGuid());

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(
            RoleErrors.RoleNotFound());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenRoleIsAlreadyActive()
    {
        var roleRepositoryMock = new Mock<IRoleRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var role = CreateRole();

        roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        var handler = new ActivateRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ActivateRoleCommand(role.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(
            RoleErrors.RoleAlreadyActive());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldActivateRole_WhenRoleIsInactive()
    {
        var roleRepositoryMock = new Mock<IRoleRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var role = CreateRole();

        role.Deactivate();

        roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        var handler = new ActivateRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ActivateRoleCommand(role.Id);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.AreEqual(
            RoleStatus.Active,
            role.Status);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static Role CreateRole()
    {
        var roleNameResult = RoleName.Create("Administrator");

        roleNameResult.ShouldSucceed();

        var roleResult = Role.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            roleNameResult.Value);

        roleResult.ShouldSucceed();

        return roleResult.Value;
    }
}