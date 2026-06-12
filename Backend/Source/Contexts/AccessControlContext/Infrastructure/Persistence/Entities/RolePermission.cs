namespace Infrastructure.Persistence.Entities
{
    public sealed class RolePermission
    {
        public Guid RoleId { get; set; }
        public string PermissionCode { get; set; } = string.Empty;
    }
}