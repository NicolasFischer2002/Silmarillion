using Domain.Aggregates.Membership.Aggregate;
using Domain.Aggregates.Role.Aggregate;
using Domain.Aggregates.Session.Aggregate;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public sealed class AccessControlDbContext : DbContext
    {
        public DbSet<Membership> Memberships => Set<Membership>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<MembershipRole> MembershipRoles => Set<MembershipRole>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Session> Sessions => Set<Session>();

        public AccessControlDbContext(DbContextOptions<AccessControlDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AccessControlDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}