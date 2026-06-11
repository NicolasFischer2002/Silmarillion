using Domain.Aggregates.Memberships.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.ToTable("memberships");

            builder.HasKey(
                membership => membership.Id);

            builder.Property(
                membership => membership.Id)
                .HasColumnName("id");

            builder.Property(
                membership => membership.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(
                membership => membership.OrganizationId)
                .HasColumnName("organization_id")
                .IsRequired();

            builder.Property(
                membership => membership.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(
                membership => membership.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(
                membership => membership.LastModifiedAt)
                .HasColumnName("last_modified_at")
                .IsRequired();

            builder.Ignore(
                membership => membership.AssignedRoles);

            builder.Ignore(
                membership => membership.DomainEvents);
        }
    }
}