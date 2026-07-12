using Domain.Aggregates.Role.Constants;

namespace API.Contexts.AccessControlContext.Contracts.Requests
{
    public sealed record RemovePermissionFromRoleRequest(PermissionCode Permission);
}