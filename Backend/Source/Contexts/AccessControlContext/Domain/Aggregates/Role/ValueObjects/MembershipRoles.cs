using Domain.Aggregates.Membership.Errors;
using SharedKernel.Results;

namespace Domain.Aggregates.Role.ValueObjects
{
    public sealed class MembershipRoles
    {
        private readonly HashSet<Guid> _roleIds = [];

        public IReadOnlyCollection<Guid> Values => _roleIds;

        public Result Add(Guid roleId)
        {
            if (roleId == Guid.Empty)
                return Result.Failure(MembershipErrors.RoleIdRequired());

            if (!_roleIds.Add(roleId))
                return Result.Failure(MembershipErrors.RoleAlreadyAssigned());

            return Result.Success();
        }

        public Result Remove(Guid roleId)
        {
            if (roleId == Guid.Empty)
                return Result.Failure(MembershipErrors.RoleIdRequired());

            if (!_roleIds.Remove(roleId))
                return Result.Failure(MembershipErrors.RoleNotAssigned());

            return Result.Success();
        }

        public bool Contains(Guid roleId)
        {
            return _roleIds.Contains(roleId);
        }

        public int Count => _roleIds.Count;
    }
}