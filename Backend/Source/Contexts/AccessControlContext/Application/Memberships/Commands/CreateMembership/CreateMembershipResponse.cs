namespace Application.Memberships.Commands.CreateMembership
{
    public sealed record CreateMembershipResponse(
        Guid MembershipId,
        Guid UserId,
        Guid OrganizationId);
}