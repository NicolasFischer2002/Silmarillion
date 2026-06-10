namespace Application.Memberships.Commands.RevokeMembership
{
    public sealed record RevokeMembershipCommand(
        Guid MembershipId);
}