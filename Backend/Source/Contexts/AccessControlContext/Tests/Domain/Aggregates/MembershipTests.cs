using Domain.Aggregates.Memberships.Aggregate;
using Domain.Aggregates.Memberships.Constants;
using Domain.Aggregates.Memberships.Errors;
using Domain.Aggregates.Memberships.Events;
using SharedTests.Assertions;

namespace Tests.Domain.Aggregates
{
    [TestClass]
    public class MembershipTests
    {
        [TestMethod]
        public void CreatePending_ShouldReturnSuccess_WhenValuesAreValid()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var organizationId = Guid.NewGuid();

            var result = Membership.CreatePending(id, userId, organizationId);

            result.ShouldSucceed();

            var membership = result.Value;

            Assert.AreEqual(id, membership.Id);
            Assert.AreEqual(userId, membership.UserId);
            Assert.AreEqual(organizationId, membership.OrganizationId);
            Assert.AreEqual(MembershipStatus.Pending, membership.Status);
            Assert.AreEqual(membership.CreatedAt, membership.LastModifiedAt);
        }

        [TestMethod]
        [DataRow(null, "11111111-1111-1111-1111-111111111111", "22222222-2222-2222-2222-222222222222")]
        [DataRow("11111111-1111-1111-1111-111111111111", null, "22222222-2222-2222-2222-222222222222")]
        [DataRow("11111111-1111-1111-1111-111111111111", "22222222-2222-2222-2222-222222222222", null)]
        public void CreatePending_ShouldReturnFailure_WhenAnyRequiredIdentifierIsMissing(
            string? id,
            string? userId,
            string? organizationId)
        {
            var parsedId = ParseOrEmpty(id);
            var parsedUserId = ParseOrEmpty(userId);
            var parsedOrganizationId = ParseOrEmpty(organizationId);

            var result = Membership.CreatePending(parsedId, parsedUserId, parsedOrganizationId);

            if (parsedId == Guid.Empty)
            {
                result.ShouldFailWith(MembershipErrors.MembershipIdRequired());
                return;
            }

            if (parsedUserId == Guid.Empty)
            {
                result.ShouldFailWith(MembershipErrors.UserIdRequired());
                return;
            }

            result.ShouldFailWith(MembershipErrors.OrganizationIdRequired());
        }

        [TestMethod]
        public void Activate_ShouldReturnSuccess_WhenStatusIsPending()
        {
            var membership = CreatePendingMembership();

            var result = membership.Activate();

            result.ShouldSucceed();

            Assert.AreEqual(MembershipStatus.Active, membership.Status);
            Assert.HasCount(1, membership.DomainEvents);
            Assert.IsInstanceOfType(membership.DomainEvents.Single(), typeof(MembershipActivatedDomainEvent));
        }

        [TestMethod]
        public void Activate_ShouldReturnSuccess_WhenStatusIsInactive()
        {
            var membership = CreatePendingMembership();
            membership.Deactivate();

            var result = membership.Activate();

            result.ShouldSucceed();

            Assert.AreEqual(MembershipStatus.Active, membership.Status);
            Assert.HasCount(2, membership.DomainEvents);
            Assert.IsInstanceOfType(membership.DomainEvents.First(), typeof(MembershipDeactivatedDomainEvent));
            Assert.IsInstanceOfType(membership.DomainEvents.Last(), typeof(MembershipActivatedDomainEvent));
        }

        [TestMethod]
        public void Activate_ShouldReturnFailure_WhenStatusIsActive()
        {
            var membership = CreatePendingMembership();
            membership.Activate();

            var result = membership.Activate();

            result.ShouldFailWith(MembershipErrors.MembershipAlreadyActive());
        }

        [TestMethod]
        public void Activate_ShouldReturnFailure_WhenStatusIsRevoked()
        {
            var membership = CreatePendingMembership();
            membership.Revoke();

            var result = membership.Activate();

            result.ShouldFailWith(MembershipErrors.CannotActivateRevokedMembership());
        }

        [TestMethod]
        public void Deactivate_ShouldReturnSuccess_WhenStatusIsPending()
        {
            var membership = CreatePendingMembership();

            var result = membership.Deactivate();

            result.ShouldSucceed();

            Assert.AreEqual(MembershipStatus.Inactive, membership.Status);
            Assert.HasCount(1, membership.DomainEvents);
            Assert.IsInstanceOfType(membership.DomainEvents.Single(), typeof(MembershipDeactivatedDomainEvent));
        }

        [TestMethod]
        public void Deactivate_ShouldReturnSuccess_WhenStatusIsActive()
        {
            var membership = CreatePendingMembership();
            membership.Activate();

            var result = membership.Deactivate();

            result.ShouldSucceed();

            Assert.AreEqual(MembershipStatus.Inactive, membership.Status);
            Assert.HasCount(2, membership.DomainEvents);
            Assert.IsInstanceOfType(membership.DomainEvents.First(), typeof(MembershipActivatedDomainEvent));
            Assert.IsInstanceOfType(membership.DomainEvents.Last(), typeof(MembershipDeactivatedDomainEvent));
        }

        [TestMethod]
        public void Deactivate_ShouldReturnFailure_WhenStatusIsInactive()
        {
            var membership = CreatePendingMembership();
            membership.Deactivate();

            var result = membership.Deactivate();

            result.ShouldFailWith(MembershipErrors.MembershipAlreadyInactive());
        }

        [TestMethod]
        public void Deactivate_ShouldReturnFailure_WhenStatusIsRevoked()
        {
            var membership = CreatePendingMembership();
            membership.Revoke();

            var result = membership.Deactivate();

            result.ShouldFailWith(MembershipErrors.CannotDeactivateRevokedMembership());
        }

        [TestMethod]
        public void Revoke_ShouldReturnSuccess_WhenStatusIsPending()
        {
            var membership = CreatePendingMembership();

            var result = membership.Revoke();

            result.ShouldSucceed();

            Assert.AreEqual(MembershipStatus.Revoked, membership.Status);
            Assert.HasCount(1, membership.DomainEvents);
            Assert.IsInstanceOfType(membership.DomainEvents.Single(), typeof(MembershipRevokedDomainEvent));
        }

        [TestMethod]
        public void Revoke_ShouldReturnSuccess_WhenStatusIsActive()
        {
            var membership = CreatePendingMembership();
            membership.Activate();

            var result = membership.Revoke();

            result.ShouldSucceed();

            Assert.AreEqual(MembershipStatus.Revoked, membership.Status);
            Assert.HasCount(2, membership.DomainEvents);
            Assert.IsInstanceOfType(membership.DomainEvents.First(), typeof(MembershipActivatedDomainEvent));
            Assert.IsInstanceOfType(membership.DomainEvents.Last(), typeof(MembershipRevokedDomainEvent));
        }

        [TestMethod]
        public void Revoke_ShouldReturnSuccess_WhenStatusIsInactive()
        {
            var membership = CreatePendingMembership();
            membership.Deactivate();

            var result = membership.Revoke();

            result.ShouldSucceed();

            Assert.AreEqual(MembershipStatus.Revoked, membership.Status);
            Assert.HasCount(2, membership.DomainEvents);
            Assert.IsInstanceOfType(membership.DomainEvents.First(), typeof(MembershipDeactivatedDomainEvent));
            Assert.IsInstanceOfType(membership.DomainEvents.Last(), typeof(MembershipRevokedDomainEvent));
        }

        [TestMethod]
        public void Revoke_ShouldReturnFailure_WhenStatusIsRevoked()
        {
            var membership = CreatePendingMembership();
            membership.Revoke();

            var result = membership.Revoke();

            result.ShouldFailWith(MembershipErrors.MembershipAlreadyRevoked());
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

        private static Guid ParseOrEmpty(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? Guid.Empty
                : Guid.Parse(value);
        }

        [TestMethod]
        public void AssignRole_ShouldReturnSuccess_WhenRoleIsNotAssigned()
        {
            var membership = CreatePendingMembership();
            var roleId = Guid.NewGuid();

            var result = membership.AssignRole(roleId);

            result.ShouldSucceed();

            Assert.HasCount(1, membership.RoleIds);
            Assert.IsTrue(membership.RoleIds.Contains(roleId));
        }

        [TestMethod]
        public void AssignRole_ShouldReturnFailure_WhenRoleIsAlreadyAssigned()
        {
            var membership = CreatePendingMembership();
            var roleId = Guid.NewGuid();

            membership.AssignRole(roleId);

            var result = membership.AssignRole(roleId);

            result.ShouldFailWith(MembershipErrors.RoleAlreadyAssigned());
        }

        [TestMethod]
        public void AssignRole_ShouldReturnFailure_WhenRoleIdIsEmpty()
        {
            var membership = CreatePendingMembership();

            var result = membership.AssignRole(Guid.Empty);

            result.ShouldFailWith(MembershipErrors.RoleIdRequired());
        }

        [TestMethod]
        public void RemoveRole_ShouldReturnSuccess_WhenRoleIsAssigned()
        {
            var membership = CreatePendingMembership();
            var roleId = Guid.NewGuid();

            membership.AssignRole(roleId);

            var result = membership.RemoveRole(roleId);

            result.ShouldSucceed();

            Assert.HasCount(0, membership.RoleIds);
            Assert.IsFalse(membership.RoleIds.Contains(roleId));
        }

        [TestMethod]
        public void RemoveRole_ShouldReturnFailure_WhenRoleIsNotAssigned()
        {
            var membership = CreatePendingMembership();
            var roleId = Guid.NewGuid();

            var result = membership.RemoveRole(roleId);

            result.ShouldFailWith(MembershipErrors.RoleNotAssigned());
        }

        [TestMethod]
        public void RemoveRole_ShouldReturnFailure_WhenRoleIdIsEmpty()
        {
            var membership = CreatePendingMembership();

            var result = membership.RemoveRole(Guid.Empty);

            result.ShouldFailWith(MembershipErrors.RoleIdRequired());
        }

        [TestMethod]
        public void AssignRole_ShouldKeepExistingRoles_WhenAddingAnotherRole()
        {
            var membership = CreatePendingMembership();
            var firstRoleId = Guid.NewGuid();
            var secondRoleId = Guid.NewGuid();

            membership.AssignRole(firstRoleId);

            var result = membership.AssignRole(secondRoleId);

            result.ShouldSucceed();

            Assert.HasCount(2, membership.RoleIds);
            Assert.IsTrue(membership.RoleIds.Contains(firstRoleId));
            Assert.IsTrue(membership.RoleIds.Contains(secondRoleId));
        }

        [TestMethod]
        public void AssignRole_ShouldUpdateLastModifiedAt_WhenRoleIsAssigned()
        {
            var membership = CreatePendingMembership();
            var originalLastModifiedAt = membership.LastModifiedAt;

            Thread.Sleep(10);

            var result = membership.AssignRole(Guid.NewGuid());

            result.ShouldSucceed();

            Assert.IsTrue(
                membership.LastModifiedAt > originalLastModifiedAt);
        }

        [TestMethod]
        public void RemoveRole_ShouldUpdateLastModifiedAt_WhenRoleIsRemoved()
        {
            var membership = CreatePendingMembership();
            var roleId = Guid.NewGuid();

            membership.AssignRole(roleId);

            var originalLastModifiedAt = membership.LastModifiedAt;

            Thread.Sleep(10);

            var result = membership.RemoveRole(roleId);

            result.ShouldSucceed();

            Assert.IsTrue(
                membership.LastModifiedAt > originalLastModifiedAt);
        }
    }
}