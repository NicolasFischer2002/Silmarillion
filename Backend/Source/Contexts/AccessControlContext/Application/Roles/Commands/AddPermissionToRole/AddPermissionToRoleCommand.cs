using Domain.Aggregates.Role.Constants;

namespace Application.Roles.Commands.AddPermissionToRole
{
    public sealed record AddPermissionToRoleCommand(
        Guid RoleId,
        PermissionCode Permission);
}