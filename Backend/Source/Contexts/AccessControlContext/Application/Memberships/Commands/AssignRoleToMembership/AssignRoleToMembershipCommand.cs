namespace Application.Memberships.Commands.AssignRoleToMembership
{
    public sealed record AssignRoleToMembershipCommand(
        Guid MembershipId,
        Guid RoleId);
}