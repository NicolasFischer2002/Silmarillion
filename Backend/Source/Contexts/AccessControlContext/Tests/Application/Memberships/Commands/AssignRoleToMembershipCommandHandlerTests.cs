using Application.Abstractions.Persistence;
using Application.Memberships.Commands.AssignRoleToMembership;
using Domain.Errors;
using Moq;
using SharedTests.Assertions;
using Domain.Aggregates.Role.Aggregate;
using Domain.Aggregates.Role.Errors;
using Domain.Aggregates.Role.ValueObjects;
using Domain.Aggregates.Membership.Aggregate;
using Domain.Aggregates.Membership.Errors;

namespace Tests.Application.Memberships.Commands
{
    [TestClass]
    public class AssignRoleToMembershipCommandHandlerTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task HandleAsync_ShouldReturnFailure_WhenMembershipDoesNotExist()
        {
            var membershipRepositoryMock = new Mock<IMembershipRepository>();

            var roleRepositoryMock = new Mock<IRoleRepository>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            membershipRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Membership?)null);

            var handler = new AssignRoleToMembershipCommandHandler(
                membershipRepositoryMock.Object,
                roleRepositoryMock.Object,
                unitOfWorkMock.Object);

            var command = new AssignRoleToMembershipCommand(
                Guid.NewGuid(),
                Guid.NewGuid());

            var result = await handler.HandleAsync(
                command,
                TestContext.CancellationToken);

            result.ShouldFailWith(MembershipErrors.MembershipNotFound());

            roleRepositoryMock.Verify(
                repository => repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);

            unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [TestMethod]
        public async Task HandleAsync_ShouldReturnFailure_WhenRoleDoesNotExist()
        {
            var membershipRepositoryMock = new Mock<IMembershipRepository>();

            var roleRepositoryMock = new Mock<IRoleRepository>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var membership = CreateMembership();

            membershipRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(membership);

            roleRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Role?)null);

            var handler = new AssignRoleToMembershipCommandHandler(
                membershipRepositoryMock.Object,
                roleRepositoryMock.Object,
                unitOfWorkMock.Object);

            var command = new AssignRoleToMembershipCommand(
                membership.Id,
                Guid.NewGuid());

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
        public async Task HandleAsync_ShouldReturnFailure_WhenRoleDoesNotBelongToMembershipOrganization()
        {
            var membershipRepositoryMock = new Mock<IMembershipRepository>();

            var roleRepositoryMock = new Mock<IRoleRepository>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var membership = CreateMembership();

            var role = CreateRole(Guid.NewGuid());

            membershipRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(membership);

            roleRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);

            var handler = new AssignRoleToMembershipCommandHandler(
                membershipRepositoryMock.Object,
                roleRepositoryMock.Object,
                unitOfWorkMock.Object);

            var command = new AssignRoleToMembershipCommand(
                membership.Id,
                role.Id);

            var result = await handler.HandleAsync(
                command,
                TestContext.CancellationToken);

            result.ShouldFailWith(AccessControlErrors.RoleDoesNotBelongToOrganization());

            unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [TestMethod]
        public async Task HandleAsync_ShouldReturnFailure_WhenRoleIsAlreadyAssigned()
        {
            var membershipRepositoryMock = new Mock<IMembershipRepository>();

            var roleRepositoryMock = new Mock<IRoleRepository>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var organizationId = Guid.NewGuid();

            var membership = CreateMembership(organizationId);

            var role = CreateRole(organizationId);

            membership.AssignRole(role.Id);

            membershipRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(membership);

            roleRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);

            var handler = new AssignRoleToMembershipCommandHandler(
                membershipRepositoryMock.Object,
                roleRepositoryMock.Object,
                unitOfWorkMock.Object);

            var command = new AssignRoleToMembershipCommand(
                membership.Id,
                role.Id);

            var result = await handler.HandleAsync(
                command,
                TestContext.CancellationToken);

            result.ShouldFailWith(MembershipErrors.RoleAlreadyAssigned());

            unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [TestMethod]
        public async Task HandleAsync_ShouldAssignRole_WhenCommandIsValid()
        {
            var membershipRepositoryMock = new Mock<IMembershipRepository>();

            var roleRepositoryMock = new Mock<IRoleRepository>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var organizationId = Guid.NewGuid();

            var membership = CreateMembership(organizationId);

            var role = CreateRole(organizationId);

            membershipRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(membership);

            roleRepositoryMock
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(role);

            var handler = new AssignRoleToMembershipCommandHandler(
                membershipRepositoryMock.Object,
                roleRepositoryMock.Object,
                unitOfWorkMock.Object);

            var command = new AssignRoleToMembershipCommand(
                membership.Id,
                role.Id);

            var result = await handler.HandleAsync(
                command,
                TestContext.CancellationToken);

            result.ShouldSucceed();

            Assert.IsTrue(membership.AssignedRoles.Values.Contains(role.Id));

            unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private static Membership CreateMembership(
            Guid? organizationId = null)
        {
            var result = Membership.CreatePending(
                Guid.NewGuid(),
                Guid.NewGuid(),
                organizationId ?? Guid.NewGuid());

            result.ShouldSucceed();

            return result.Value;
        }

        private static Role CreateRole(
            Guid organizationId)
        {
            var roleNameResult =
                RoleName.Create("Administrator");

            roleNameResult.ShouldSucceed();

            var roleResult = Role.Create(
                Guid.NewGuid(),
                organizationId,
                roleNameResult.Value);

            roleResult.ShouldSucceed();

            return roleResult.Value;
        }
    }
}