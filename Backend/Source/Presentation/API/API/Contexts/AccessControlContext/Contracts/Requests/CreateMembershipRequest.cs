namespace API.Contexts.AccessControlContext.Contracts.Requests
{
    public sealed record CreateMembershipRequest(
        Guid UserId,
        Guid OrganizationId);
}