namespace API.Contexts.AccessControlContext.Contracts.Requests
{
    public sealed record CreateRoleRequest(
        Guid OrganizationId,
        string Name);
}