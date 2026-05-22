using Domain.Aggregates.Roles.Aggregate;
using Domain.Aggregates.Roles.Constants;
using Domain.Aggregates.Roles.Errors;
using Domain.Aggregates.Roles.Events;
using Domain.Aggregates.Roles.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.Aggregates;

[TestClass]
public class RoleTests
{
    [TestMethod]
    public void Create_ShouldReturnSuccess_WhenValuesAreValid()
    {
        var id = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var roleName = CreateRoleName("Administrator");

        var result = Role.Create(id, organizationId, roleName);

        result.ShouldSucceed();

        var role = result.Value;

        Assert.AreEqual(id, role.Id);
        Assert.AreEqual(organizationId, role.OrganizationId);
        Assert.AreEqual(roleName, role.Name);
        Assert.AreEqual(RoleStatus.Active, role.Status);
        Assert.HasCount(0, role.Permissions);
        Assert.AreEqual(role.CreatedAt, role.LastModifiedAt);
        Assert.HasCount(1, role.DomainEvents);
        Assert.IsInstanceOfType(role.DomainEvents.Single(), typeof(RoleCreatedDomainEvent));
    }

    [TestMethod]
    [DataRow("00000000-0000-0000-0000-000000000000", "22222222-2222-2222-2222-222222222222")]
    [DataRow("11111111-1111-1111-1111-111111111111", "00000000-0000-0000-0000-000000000000")]
    public void Create_ShouldReturnFailure_WhenAnyRequiredIdentifierIsMissing(
        string id,
        string organizationId)
    {
        var parsedId = Guid.Parse(id);
        var parsedOrganizationId = Guid.Parse(organizationId);
        var roleName = CreateRoleName("Administrator");

        var result = Role.Create(parsedId, parsedOrganizationId, roleName);

        if (parsedId == Guid.Empty)
        {
            result.ShouldFailWith(RoleErrors.RoleIdRequired());
            return;
        }

        result.ShouldFailWith(RoleErrors.OrganizationIdRequired());
    }

    [TestMethod]
    public void AddPermission_ShouldReturnSuccess_WhenPermissionIsNotAssigned()
    {
        var role = CreateActiveRole();

        var result = role.AddPermission(PermissionCode.OrganizationRead);

        result.ShouldSucceed();

        Assert.HasCount(1, role.Permissions);
        Assert.IsTrue(role.Permissions.Contains(PermissionCode.OrganizationRead));
    }

    [TestMethod]
    public void AddPermission_ShouldReturnFailure_WhenPermissionIsAlreadyAssigned()
    {
        var role = CreateActiveRole();
        role.AddPermission(PermissionCode.OrganizationRead);

        var result = role.AddPermission(PermissionCode.OrganizationRead);

        result.ShouldFailWith(RoleErrors.PermissionAlreadyAssigned());
    }

    [TestMethod]
    public void RemovePermission_ShouldReturnSuccess_WhenPermissionIsAssigned()
    {
        var role = CreateActiveRole();
        role.AddPermission(PermissionCode.OrganizationRead);

        var result = role.RemovePermission(PermissionCode.OrganizationRead);

        result.ShouldSucceed();

        Assert.HasCount(0, role.Permissions);
        Assert.IsFalse(role.Permissions.Contains(PermissionCode.OrganizationRead));
    }

    [TestMethod]
    public void RemovePermission_ShouldReturnFailure_WhenPermissionIsNotAssigned()
    {
        var role = CreateActiveRole();

        var result = role.RemovePermission(PermissionCode.OrganizationRead);

        result.ShouldFailWith(RoleErrors.PermissionNotAssigned());
    }

    [TestMethod]
    public void Rename_ShouldReturnSuccess_WhenRoleNameIsValid()
    {
        var role = CreateActiveRole();
        var newName = CreateRoleName("Supervisor");

        var result = role.Rename(newName);

        result.ShouldSucceed();

        Assert.AreEqual(newName, role.Name);
    }

    [TestMethod]
    public void Deactivate_ShouldReturnSuccess_WhenStatusIsActive()
    {
        var role = CreateActiveRole();

        var result = role.Deactivate();

        result.ShouldSucceed();

        Assert.AreEqual(RoleStatus.Inactive, role.Status);
        Assert.HasCount(2, role.DomainEvents);
        Assert.IsInstanceOfType(role.DomainEvents.First(), typeof(RoleCreatedDomainEvent));
        Assert.IsInstanceOfType(role.DomainEvents.Last(), typeof(RoleDeactivatedDomainEvent));
    }

    [TestMethod]
    public void Deactivate_ShouldReturnFailure_WhenStatusIsInactive()
    {
        var role = CreateActiveRole();
        role.Deactivate();

        var result = role.Deactivate();

        result.ShouldFailWith(RoleErrors.RoleAlreadyInactive());
    }

    [TestMethod]
    public void Activate_ShouldReturnSuccess_WhenStatusIsInactive()
    {
        var role = CreateActiveRole();
        role.Deactivate();

        var result = role.Activate();

        result.ShouldSucceed();

        Assert.AreEqual(RoleStatus.Active, role.Status);
        Assert.HasCount(2, role.DomainEvents);
        Assert.IsInstanceOfType(role.DomainEvents.First(), typeof(RoleCreatedDomainEvent));
        Assert.IsInstanceOfType(role.DomainEvents.Last(), typeof(RoleDeactivatedDomainEvent));
    }

    [TestMethod]
    public void Activate_ShouldReturnFailure_WhenStatusIsActive()
    {
        var role = CreateActiveRole();

        var result = role.Activate();

        result.ShouldFailWith(RoleErrors.RoleAlreadyActive());
    }

    private static Role CreateActiveRole()
    {
        var result = Role.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            CreateRoleName("Administrator"));

        result.ShouldSucceed();
        return result.Value;
    }

    private static RoleName CreateRoleName(string value)
    {
        var result = RoleName.Create(value);
        result.ShouldSucceed();
        return result.Value;
    }
}