using Application.Abstractions.Persistence;
using Application.Permissions.Queries.GetEffectivePermissions;
using Domain.Aggregates.Membership.Aggregate;
using Domain.Aggregates.Role.Aggregate;
using Domain.Aggregates.Role.Constants;
using Domain.Aggregates.Role.ValueObjects;
using Moq;
using SharedTests.Assertions;

namespace Tests.Application.Permissions.Queries.GetEffectivePermissions;

[TestClass]
public class GetEffectivePermissionsQueryHandlerTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public async Task HandleAsync_ShouldReturnEmptyPermissions_WhenMembershipDoesNotExist()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var roleRepositoryMock = new Mock<IRoleRepository>();

        membershipRepositoryMock
            .Setup(repository => repository.GetByUserAndOrganizationAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Membership?)null);

        var handler = new GetEffectivePermissionsQueryHandler(
            membershipRepositoryMock.Object,
            roleRepositoryMock.Object);

        var query = new GetEffectivePermissionsQuery(
            Guid.NewGuid(),
            Guid.NewGuid());

        var response = await handler.HandleAsync(
            query,
            TestContext.CancellationToken);

        Assert.IsEmpty(response.Permissions);

        roleRepositoryMock.Verify(
            repository => repository.GetByIdsAsync(
                It.IsAny<IReadOnlyCollection<Guid>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnEmptyPermissions_WhenMembershipIsNotActive()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var roleRepositoryMock = new Mock<IRoleRepository>();

        var membership = CreatePendingMembership();

        membershipRepositoryMock
            .Setup(repository => repository.GetByUserAndOrganizationAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var handler = new GetEffectivePermissionsQueryHandler(
            membershipRepositoryMock.Object,
            roleRepositoryMock.Object);

        var query = new GetEffectivePermissionsQuery(
            membership.UserId,
            membership.OrganizationId);

        var response = await handler.HandleAsync(
            query,
            TestContext.CancellationToken);

        Assert.IsEmpty(response.Permissions);

        roleRepositoryMock.Verify(
            repository => repository.GetByIdsAsync(
                It.IsAny<IReadOnlyCollection<Guid>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnPermissionsFromActiveRoles()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var roleRepositoryMock = new Mock<IRoleRepository>();

        var role1 = CreateRole("Administrator");

        role1.AddPermission(PermissionCode.RoleCreate);
        role1.AddPermission(PermissionCode.RoleRead);

        var role2 = CreateRole("Auditor");

        role2.AddPermission(PermissionCode.MembershipRead);

        var membership = CreateActiveMembership();

        membership.AssignRole(role1.Id);
        membership.AssignRole(role2.Id);

        membershipRepositoryMock
            .Setup(repository => repository.GetByUserAndOrganizationAsync(
                membership.UserId,
                membership.OrganizationId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        roleRepositoryMock
            .Setup(repository => repository.GetByIdsAsync(
                It.IsAny<IReadOnlyCollection<Guid>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([role1, role2]);

        var handler = new GetEffectivePermissionsQueryHandler(
            membershipRepositoryMock.Object,
            roleRepositoryMock.Object);

        var query = new GetEffectivePermissionsQuery(
            membership.UserId,
            membership.OrganizationId);

        var response = await handler.HandleAsync(
            query,
            TestContext.CancellationToken);

        Assert.HasCount(3, response.Permissions);

        CollectionAssert.Contains(
            response.Permissions.ToList(),
            PermissionCode.RoleCreate);

        CollectionAssert.Contains(
            response.Permissions.ToList(),
            PermissionCode.RoleRead);

        CollectionAssert.Contains(
            response.Permissions.ToList(),
            PermissionCode.MembershipRead);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldIgnoreInactiveRoles()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var roleRepositoryMock = new Mock<IRoleRepository>();

        var activeRole = CreateRole("Active Role");

        activeRole.AddPermission(PermissionCode.RoleRead);

        var inactiveRole = CreateRole("Inactive Role");

        inactiveRole.AddPermission(PermissionCode.RoleCreate);

        inactiveRole.Deactivate();

        var membership = CreateActiveMembership();

        membership.AssignRole(activeRole.Id);
        membership.AssignRole(inactiveRole.Id);

        membershipRepositoryMock
            .Setup(repository => repository.GetByUserAndOrganizationAsync(
                membership.UserId,
                membership.OrganizationId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        roleRepositoryMock
            .Setup(repository => repository.GetByIdsAsync(
                It.IsAny<IReadOnlyCollection<Guid>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([activeRole, inactiveRole]);

        var handler = new GetEffectivePermissionsQueryHandler(
            membershipRepositoryMock.Object,
            roleRepositoryMock.Object);

        var query = new GetEffectivePermissionsQuery(
            membership.UserId,
            membership.OrganizationId);

        var response = await handler.HandleAsync(
            query,
            TestContext.CancellationToken);

        CollectionAssert.Contains(
            response.Permissions.ToList(),
            PermissionCode.RoleRead);

        CollectionAssert.DoesNotContain(
            response.Permissions.ToList(),
            PermissionCode.RoleCreate);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnDistinctPermissions_WhenMultipleRolesContainSamePermission()
    {
        var membershipRepositoryMock = new Mock<IMembershipRepository>();

        var roleRepositoryMock = new Mock<IRoleRepository>();

        var role1 = CreateRole("Role A");

        role1.AddPermission(PermissionCode.RoleRead);
        role1.AddPermission(PermissionCode.MembershipRead);

        var role2 = CreateRole("Role B");

        role2.AddPermission(PermissionCode.RoleRead);
        role2.AddPermission(PermissionCode.OrganizationRead);

        var membership = CreateActiveMembership();

        membership.AssignRole(role1.Id);
        membership.AssignRole(role2.Id);

        membershipRepositoryMock
            .Setup(repository => repository.GetByUserAndOrganizationAsync(
                membership.UserId,
                membership.OrganizationId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        roleRepositoryMock
            .Setup(repository => repository.GetByIdsAsync(
                It.IsAny<IReadOnlyCollection<Guid>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([role1, role2]);

        var handler = new GetEffectivePermissionsQueryHandler(
            membershipRepositoryMock.Object,
            roleRepositoryMock.Object);

        var query = new GetEffectivePermissionsQuery(
            membership.UserId,
            membership.OrganizationId);

        var response = await handler.HandleAsync(
            query,
            TestContext.CancellationToken);

        Assert.AreEqual(
            1,
            response.Permissions.Count(
                permission => permission == PermissionCode.RoleRead));

        Assert.HasCount(
            3,
            response.Permissions);
    }

    private static Membership CreatePendingMembership()
    {
        var result = Membership.CreatePending(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid());

        result.ShouldSucceed();

        return result.Value;
    }

    private static Membership CreateActiveMembership()
    {
        var membership = CreatePendingMembership();

        membership.Activate();

        return membership;
    }

    private static Role CreateRole(string name)
    {
        var result = Role.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            RoleName.Create(name).Value);

        result.ShouldSucceed();

        return result.Value;
    }
}