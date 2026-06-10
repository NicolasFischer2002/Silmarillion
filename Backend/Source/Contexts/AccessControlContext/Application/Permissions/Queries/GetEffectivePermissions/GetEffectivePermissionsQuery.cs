namespace Application.Permissions.Queries.GetEffectivePermissions
{
    public sealed record GetEffectivePermissionsQuery(
        Guid UserId,
        Guid OrganizationId);
}