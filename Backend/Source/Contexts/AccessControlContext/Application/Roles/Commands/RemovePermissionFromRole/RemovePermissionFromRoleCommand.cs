using Domain.Aggregates.Roles.Constants;

namespace Application.Roles.Commands.RemovePermissionFromRole
{
    public sealed record RemovePermissionFromRoleCommand(
        Guid RoleId,
        PermissionCode Permission);
}