using Domain.Aggregates.Role.Constants;
using Domain.Aggregates.Role.Errors;
using SharedKernel.Results;

namespace Domain.Aggregates.Role.ValueObjects
{
    public sealed class RolePermissions
    {
        private readonly HashSet<PermissionCode> _permissions = [];
        public IReadOnlyCollection<PermissionCode> Values => _permissions;

        public Result Add(PermissionCode permission)
        {
            if (!_permissions.Add(permission))
                return Result.Failure(RoleErrors.PermissionAlreadyAssigned());

            return Result.Success();
        }

        public Result Remove(PermissionCode permission)
        {
            if (!_permissions.Remove(permission))
                return Result.Failure(RoleErrors.PermissionNotAssigned());

            return Result.Success();
        }

        public bool Contains(PermissionCode permission)
        {
            return _permissions.Contains(permission);
        }

        public int Count => _permissions.Count;
    }
}