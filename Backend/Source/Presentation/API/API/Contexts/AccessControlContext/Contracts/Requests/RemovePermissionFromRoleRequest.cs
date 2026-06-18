using Domain.Aggregates.Roles.Constants;

namespace API.Contexts.AccessControlContext.Contracts.Requests
{
    public sealed record RemovePermissionFromRoleRequest(
        Guid RoleId,
        PermissionCode Permission);
}