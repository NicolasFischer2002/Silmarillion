using Application.Abstractions.Persistence;
using Application.Roles.Commands.AddPermissionToRole;
using Domain.Aggregates.Roles.Aggregate;
using Domain.Aggregates.Roles.Constants;
using Domain.Aggregates.Roles.Errors;
using Domain.Aggregates.Roles.ValueObjects;
using Moq;
using SharedTests.Assertions;

namespace Tests.Application.Roles.Commands;

[TestClass]
public class AddPermissionToRoleCommandHandlerTests
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

        var handler = new AddPermissionToRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new AddPermissionToRoleCommand(
            Guid.NewGuid(),
            PermissionCode.MembershipRead);

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
    public async Task HandleAsync_ShouldReturnFailure_WhenPermissionIsAlreadyAssigned()
    {
        var roleRepositoryMock = new Mock<IRoleRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var role = CreateRole();

        role.AddPermission(PermissionCode.MembershipRead);

        roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        var handler = new AddPermissionToRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new AddPermissionToRoleCommand(
            role.Id,
            PermissionCode.MembershipRead);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldFailWith(RoleErrors.PermissionAlreadyAssigned());

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldAssignPermission_WhenCommandIsValid()
    {
        var roleRepositoryMock = new Mock<IRoleRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var role = CreateRole();

        roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        var handler = new AddPermissionToRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new AddPermissionToRoleCommand(
            role.Id,
            PermissionCode.MembershipRead);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.IsTrue(role.Permissions.Contains(PermissionCode.MembershipRead));

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldPersistPermission_WhenCommandIsValid()
    {
        var roleRepositoryMock = new Mock<IRoleRepository>();

        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var role = CreateRole();

        roleRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        var handler = new AddPermissionToRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new AddPermissionToRoleCommand(
            role.Id,
            PermissionCode.OrganizationRead);

        var result = await handler.HandleAsync(
            command,
            TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.HasCount(1, role.Permissions);

        Assert.IsTrue(role.Permissions.Contains(PermissionCode.OrganizationRead));
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