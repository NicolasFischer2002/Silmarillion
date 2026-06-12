using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("role_permissions");

            builder.HasKey(
                x => new
                {
                    x.RoleId,
                    x.PermissionCode
                });

            builder.Property(x => x.RoleId)
                .HasColumnName("role_id");

            builder.Property(x => x.PermissionCode)
                .HasColumnName("permission_code")
                .HasMaxLength(100);
        }
    }
}