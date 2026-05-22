namespace Application.Roles.Commands.CreateRole
{
    public sealed record CreateRoleCommand(
        Guid OrganizationId,
        string Name);
}