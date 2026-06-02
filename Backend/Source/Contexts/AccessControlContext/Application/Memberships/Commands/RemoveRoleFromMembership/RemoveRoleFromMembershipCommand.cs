namespace Application.Memberships.Commands.RemoveRoleFromMembership
{
    public sealed record RemoveRoleFromMembershipCommand(
        Guid MembershipId,
        Guid RoleId);
}