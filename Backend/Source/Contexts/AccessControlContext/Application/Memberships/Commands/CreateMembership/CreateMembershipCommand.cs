namespace Application.Memberships.Commands.CreateMembership
{
    public sealed record CreateMembershipCommand(
        Guid UserId,
        Guid OrganizationId);
}