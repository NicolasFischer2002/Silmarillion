using Domain.Aggregates.Roles.Constants;

namespace API.Contexts.AccessControlContext.Contracts.Requests
{
    public sealed record AddPermissionToRoleRequest(PermissionCode Permission);
}