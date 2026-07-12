using Application.Abstractions.Persistence;
using Application.Roles.Commands.RemovePermissionFromRole;
using Domain.Aggregates.Role.Aggregate;
using Domain.Aggregates.Role.Constants;
using Domain.Aggregates.Role.Errors;
using Domain.Aggregates.Role.ValueObjects;
using Moq;
using SharedTests.Assertions;

namespace Tests.Application.Roles.Commands;

[TestClass]
public class RemovePermissionFromRoleCommandHandlerTests
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

        var handler = new RemovePermissionFromRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RemovePermissionFromRoleCommand(
            Guid.NewGuid(),
            PermissionCode.RoleCreate);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(RoleErrors.RoleNotFound());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenPermissionIsNotAssigned()
    {
        var roleRepositoryMock = new Mock<IRoleRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var role = CreateRole();

        roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        var handler = new RemovePermissionFromRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RemovePermissionFromRoleCommand(
            role.Id,
            PermissionCode.RoleCreate);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(RoleErrors.PermissionNotAssigned());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldRemovePermission_WhenPermissionIsAssigned()
    {
        var roleRepositoryMock = new Mock<IRoleRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var role = CreateRole();

        role.AddPermission(PermissionCode.RoleCreate);

        roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        var handler = new RemovePermissionFromRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RemovePermissionFromRoleCommand(
            role.Id,
            PermissionCode.RoleCreate);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.IsFalse(role.Permissions.Contains(PermissionCode.RoleCreate));

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldPersistChanges_WhenPermissionIsRemoved()
    {
        var roleRepositoryMock = new Mock<IRoleRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var role = CreateRole();

        role.AddPermission(PermissionCode.RoleCreate);

        roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        var handler = new RemovePermissionFromRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new RemovePermissionFromRoleCommand(
            role.Id,
            PermissionCode.RoleCreate);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

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