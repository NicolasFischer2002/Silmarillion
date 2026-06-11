namespace Infrastructure.Persistence.Entities
{
    public sealed class MembershipRole
    {
        public Guid MembershipId { get; set; }
        public Guid RoleId { get; set; }
    }
}