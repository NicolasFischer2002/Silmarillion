using Domain.Aggregates.Role.Constants;

namespace Application.Roles.Commands.RemovePermissionFromRole
{
    public sealed record RemovePermissionFromRoleCommand(
        Guid RoleId,
        PermissionCode Permission);
}