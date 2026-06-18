namespace API.Contexts.AccessControlContext.Contracts.Requests
{
    public sealed record RenameRoleRequest(
        Guid RoleId, 
        string NewName);
}