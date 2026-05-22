using Domain.Aggregates.Roles.Constants;
using Domain.Aggregates.Roles.Errors;
using Domain.Aggregates.Roles.Events;
using Domain.Aggregates.Roles.ValueObjects;
using SharedKernel.DomainEvents;
using SharedKernel.Results;

namespace Domain.Aggregates.Roles.Aggregate
{
    public sealed class Role : Entity
    {
        public Guid Id { get; }

        public Guid OrganizationId { get; }

        public RoleName Name { get; private set; }

        public RoleStatus Status { get; private set; }

        public IReadOnlyCollection<PermissionCode> Permissions =>
            _permissions;

        private readonly HashSet<PermissionCode> _permissions = [];

        public DateTime CreatedAt { get; }

        public DateTime LastModifiedAt { get; private set; }

        private Role(
            Guid id,
            Guid organizationId,
            RoleName name,
            RoleStatus status,
            DateTime createdAt,
            DateTime lastModifiedAt)
        {
            Id = id;
            OrganizationId = organizationId;
            Name = name;
            Status = status;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
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
                now,
                now
            );

            role.AddDomainEvent(
                new RoleCreatedDomainEvent(
                    role.Id,
                    role.OrganizationId,
                    now));

            return Result<Role>.Success(role);
        }

        public Result AddPermission(PermissionCode permission)
        {
            if (_permissions.Contains(permission))
                return Result.Failure(RoleErrors.PermissionAlreadyAssigned());

            _permissions.Add(permission);

            LastModifiedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result RemovePermission(PermissionCode permission)
        {
            if (!_permissions.Contains(permission))
                return Result.Failure(RoleErrors.PermissionNotAssigned());

            _permissions.Remove(permission);

            LastModifiedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result Rename(RoleName name)
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
            {
                return Result.Failure(
                    RoleErrors.RoleAlreadyActive());
            }

            Status = RoleStatus.Active;

            LastModifiedAt = DateTime.UtcNow;

            return Result.Success();
        }
    }
}