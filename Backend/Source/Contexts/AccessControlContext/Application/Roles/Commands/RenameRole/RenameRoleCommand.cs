namespace Application.Roles.Commands.RenameRole
{
    public sealed record RenameRoleCommand(
        Guid RoleId,
        string Name);
}