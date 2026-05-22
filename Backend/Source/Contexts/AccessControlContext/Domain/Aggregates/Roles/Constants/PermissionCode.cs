namespace Domain.Aggregates.Roles.Constants
{
    public enum PermissionCode
    {
        OrganizationRead = 1,
        OrganizationUpdate = 2,

        MembershipRead = 3,
        MembershipInvite = 4,
        MembershipDeactivate = 5,

        RoleRead = 6,
        RoleCreate = 7,
        RoleUpdate = 8
    }
}