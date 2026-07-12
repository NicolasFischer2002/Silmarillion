using Domain.Aggregates.Role.Aggregate;
using Domain.Aggregates.Role.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class RoleConfiguration
    : IEntityTypeConfiguration<Role>
    {
        public void Configure(
            EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            builder.HasKey(
                role => role.Id);

            builder.Property(
                role => role.Id)
                .HasColumnName("id");

            builder.Property(
                role => role.OrganizationId)
                .HasColumnName("organization_id")
                .IsRequired();

            builder.Property(
                role => role.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired()
                .HasConversion(
                    roleName => roleName.Value,
                    value => RoleName.Create(value).Value);

            builder.Property(
                role => role.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(
                role => role.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(
                role => role.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .IsRequired();

            builder.Ignore(
                role => role.Permissions);

            builder.Ignore(
                role => role.DomainEvents);
        }
    }
}