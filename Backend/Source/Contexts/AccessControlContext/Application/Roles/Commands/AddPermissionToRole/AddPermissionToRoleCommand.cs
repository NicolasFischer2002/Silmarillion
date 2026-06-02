using Domain.Aggregates.Roles.Constants;

namespace Application.Roles.Commands.AddPermissionToRole
{
    public sealed record AddPermissionToRoleCommand(
        Guid RoleId,
        PermissionCode Permission);
}