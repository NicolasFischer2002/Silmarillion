using Domain.Aggregates.Role.Constants;

namespace Application.Permissions.Queries.GetEffectivePermissions
{
    public sealed record GetEffectivePermissionsResponse(
        IReadOnlyCollection<PermissionCode> Permissions);
}