using Domain.Aggregates.Role.Constants;
using Domain.Aggregates.Role.Errors;
using Domain.Aggregates.Role.ValueObjects;
using Domain.Aggregates.Roles.Constants;
using Domain.Aggregates.Role.Events;
using SharedKernel.DomainEvents;
using SharedKernel.Results;

namespace Domain.Aggregates.Role.Aggregate
{
    public sealed class Role : Entity
    {
        public Guid Id { get; }
        public Guid OrganizationId { get; }
        public RoleName Name { get; private set; }
        public RoleStatus Status { get; private set; }
        public RolePermissions Permissions { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime LastModifiedAt { get; private set; }

        private Role() { }

        private Role(
            Guid id,
            Guid organizationId,
            RoleName name,
            RoleStatus status,
            RolePermissions permissions,
            DateTime createdAt,
            DateTime lastModifiedAt)
        {
            Id = id;
            OrganizationId = organizationId;
            Name = name;
            Status = status;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
            Permissions = permissions;
        }

        public static Result<Role> Create(
            Guid id,
            Guid organizationId,
            RoleName name)
        {
            if (id == Guid.Empty)
                return Result<Role>.Failure(RoleErrors.RoleIdRequired());

            if (organizationId == Guid.Empty)
                return Result<Role>.Failure(RoleErrors.OrganizationIdRequired());

            var now = DateTime.UtcNow;

            var role = new Role(
                id,
                organizationId,
                name,
                RoleStatus.Active,
                new RolePermissions(),
                now,
                now);

            role.AddDomainEvent(
                new RoleCreatedDomainEvent(
                    role.Id,
                    role.OrganizationId,
                    now));

            return Result<Role>.Success(role);
        }

        public Result AddPermission(
            PermissionCode permission)
        {
            var result = Permissions.Add(permission);

            if (result.IsFailure)
                return result;

            LastModifiedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result RemovePermission(
            PermissionCode permission)
        {
            var result = Permissions.Remove(permission);

            if (result.IsFailure)
                return result;

            LastModifiedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result Rename(
            RoleName name)
        {
            Name = name;

            LastModifiedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result Deactivate()
        {
            if (Status == RoleStatus.Inactive)
                return Result.Failure(RoleErrors.RoleAlreadyInactive());

            Status = RoleStatus.Inactive;

            var now = DateTime.UtcNow;

            LastModifiedAt = now;

            AddDomainEvent(
                new RoleDeactivatedDomainEvent(
                    Id,
                    OrganizationId,
                    now));

            return Result.Success();
        }

        public Result Activate()
        {
            if (Status == RoleStatus.Active)
                return Result.Failure(RoleErrors.RoleAlreadyActive());

            Status = RoleStatus.Active;

            LastModifiedAt = DateTime.UtcNow;

            return Result.Success();
        }
    }
}