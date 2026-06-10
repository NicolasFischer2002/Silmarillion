using Application.Abstractions.Persistence;
using Application.Roles.Commands.CreateRole;
using Domain.Aggregates.Roles.Aggregate;
using Domain.Aggregates.Roles.Errors;
using Moq;
using SharedTests.Assertions;

namespace Tests.Application.Roles.Commands;

[TestClass]
public class CreateRoleCommandHandlerTests
{
    [TestMethod]
    public async Task HandleAsync_ShouldReturnSuccess_WhenCommandIsValid()
    {
        var roleRepositoryMock =
            new Mock<IRoleRepository>();

        var unitOfWorkMock =
            new Mock<IUnitOfWork>();

        var handler = new CreateRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateRoleCommand(
            Guid.NewGuid(),
            "Administrator");

        var result = await handler.HandleAsync(command, TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.IsNotNull(result.Value);

        Assert.AreEqual(
            "Administrator",
            result.Value.Name);

        roleRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Role>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public async Task HandleAsync_ShouldReturnFailure_WhenRoleNameIsMissing(
        string? roleName)
    {
        var roleRepositoryMock =
            new Mock<IRoleRepository>();

        var unitOfWorkMock =
            new Mock<IUnitOfWork>();

        var handler = new CreateRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateRoleCommand(
            Guid.NewGuid(),
            roleName!);

        var result = await handler.HandleAsync(command, TestContext.CancellationToken);

        result.ShouldFailWith(
            RoleNameErrors.NameRequired());

        roleRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Role>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenRoleNameIsNull()
    {
        var roleRepositoryMock =
            new Mock<IRoleRepository>();

        var unitOfWorkMock =
            new Mock<IUnitOfWork>();

        var handler = new CreateRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateRoleCommand(
            Guid.NewGuid(),
            null!);

        var result = await handler.HandleAsync(command, TestContext.CancellationToken);

        result.ShouldFailWith(
            RoleNameErrors.NameRequired());

        roleRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Role>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnFailure_WhenRoleNameIsTooShort()
    {
        var roleRepositoryMock =
            new Mock<IRoleRepository>();

        var unitOfWorkMock =
            new Mock<IUnitOfWork>();

        var handler = new CreateRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateRoleCommand(
            Guid.NewGuid(),
            "ab");

        var result = await handler.HandleAsync(command, TestContext.CancellationToken);

        result.ShouldFailWith(
            RoleNameErrors.NameTooShort(3));

        roleRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Role>(),
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
        var roleRepositoryMock =
            new Mock<IRoleRepository>();

        var unitOfWorkMock =
            new Mock<IUnitOfWork>();

        var handler = new CreateRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateRoleCommand(
            Guid.Empty,
            "Administrator");

        var result = await handler.HandleAsync(command, TestContext.CancellationToken);

        result.ShouldFailWith(
            RoleErrors.OrganizationIdRequired());

        roleRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Role>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldPersistRole_WhenCommandIsValid()
    {
        var roleRepositoryMock =
            new Mock<IRoleRepository>();

        var unitOfWorkMock =
            new Mock<IUnitOfWork>();

        Role? capturedRole = null;

        roleRepositoryMock
            .Setup(repository => repository.AddAsync(
                It.IsAny<Role>(),
                It.IsAny<CancellationToken>()))
            .Callback<Role, CancellationToken>((role, _) =>
            {
                capturedRole = role;
            });

        var handler = new CreateRoleCommandHandler(
            roleRepositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateRoleCommand(
            Guid.NewGuid(),
            "Manager");

        var result = await handler.HandleAsync(command, TestContext.CancellationToken);

        result.ShouldSucceed();

        Assert.IsNotNull(capturedRole);

        Assert.AreEqual(
            command.OrganizationId,
            capturedRole.OrganizationId);

        Assert.AreEqual(
            "Manager",
            capturedRole.Name.Value);
    }

    public TestContext TestContext { get; set; }
}