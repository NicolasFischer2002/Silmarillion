using Domain.Aggregates.Roles.Constants;

namespace Application.Permissions.Queries.GetEffectivePermissions
{
    public sealed record GetEffectivePermissionsResponse(
        IReadOnlyCollection<PermissionCode> Permissions);
}