using Domain.Aggregates.Session.Aggregate;
using Domain.Aggregates.Session.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("sessions");

            builder.HasKey(
                session => session.Id);

            builder.Property(
                session => session.Id)
                .HasColumnName("id");

            builder.Property(
                session => session.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(
                session => session.RefreshTokenHash)
                .HasColumnName("refresh_token_hash")
                .HasConversion(
                    hash => hash.ToString(),
                    value => RefreshTokenHash.Create(value).Value)
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(
                session => session.IPAddress)
                .HasColumnName("ip_address")
                .HasConversion(
                    ip => ip.ToString(),
                    value => IPAddress.Create(value).Value)
                .HasMaxLength(45)
                .IsRequired();

            builder.Property(
                session => session.UserAgent)
                .HasColumnName("user_agent")
                .HasConversion(
                    userAgent => userAgent.ToString(),
                    value => UserAgent.Create(value).Value)
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(
                session => session.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(
                session => session.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired();

            builder.Property(
                session => session.RevokedAt)
                .HasColumnName("revoked_at");

            builder.Property(
                session => session.LastActivityAt)
                .HasColumnName("last_activity_at");

            builder.Ignore(
                session => session.DomainEvents);
        }
    }
}