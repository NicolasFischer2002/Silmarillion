using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class MembershipRoleConfiguration : IEntityTypeConfiguration<MembershipRole>
    {
        public void Configure(EntityTypeBuilder<MembershipRole> builder)
        {
            builder.ToTable("membership_roles");

            builder.HasKey(
                x => new
                {
                    x.MembershipId,
                    x.RoleId
                });

            builder.Property(x => x.MembershipId)
                .HasColumnName("membership_id");

            builder.Property(x => x.RoleId)
                .HasColumnName("role_id");
        }
    }
}