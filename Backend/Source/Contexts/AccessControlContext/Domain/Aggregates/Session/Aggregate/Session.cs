using Domain.Aggregates.Session.Errors;
using Domain.Aggregates.Session.Events;
using Domain.Aggregates.Session.ValueObjects;
using SharedKernel.DomainEvents;
using SharedKernel.Results;

namespace Domain.Aggregates.Session.Aggregate
{
    public sealed class Session : Entity    
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public RefreshTokenHash RefreshTokenHash { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public DateTime? LastActivityAt { get; private set; }
        public IPAddress IPAddress { get; private set; }
        public UserAgent UserAgent { get; private set; }

        private Session() { }

        private Session(
            Guid id,
            Guid userId,
            RefreshTokenHash refreshTokenHash,
            DateTime createdAt,
            DateTime expiresAt,
            DateTime? revokedAt,
            DateTime? lastActivityAt,
            IPAddress ipAddress,
            UserAgent userAgent)
        {
            Id = id; 
            UserId = userId;
            RefreshTokenHash = refreshTokenHash;
            UserAgent = userAgent;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
            RevokedAt = revokedAt;
            LastActivityAt = lastActivityAt;
            IPAddress = ipAddress;
            UserAgent = userAgent;
        }

        public static Result<Session> Create(
            Guid id,
            Guid userId,
            RefreshTokenHash refreshTokenHash,
            DateTime expiresAt,
            IPAddress ipAddress,
            UserAgent userAgent)
        {
            if (id == Guid.Empty)
                return Result<Session>.Failure(SessionErrors.SessionIdInvalid());

            if (userId == Guid.Empty)
                return Result<Session>.Failure(SessionErrors.UserIdInvalid());

            if (refreshTokenHash is null)
                return Result<Session>.Failure(SessionErrors.RefreshTokenHashRequired());

            if (ipAddress is null)
                return Result<Session>.Failure(SessionErrors.IpAddressRequired());

            if (userAgent is null)
                return Result<Session>.Failure(SessionErrors.UserAgentRequired());

            var now = DateTime.UtcNow;

            if (expiresAt <= now)
                return Result<Session>.Failure(SessionErrors.ExpirationDateInvalid());

            var session = new Session(
                id,
                userId,
                refreshTokenHash,
                now,
                expiresAt,
                null,
                null,
                ipAddress,
                userAgent);

            session.AddDomainEvent(
                new SessionCreatedDomainEvent(
                    session.Id,
                    session.UserId,
                    session.CreatedAt));

            return Result<Session>.Success(session);
        }

        public Result Touch()
        {
            if (!IsActive())
                return Result.Failure(SessionErrors.SessionNotActive());

            LastActivityAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result Revoke()
        {
            if (IsRevoked())
                return Result.Failure(SessionErrors.SessionAlreadyRevoked());

            if (IsExpired())
                return Result.Failure(SessionErrors.SessionAlreadyExpired());

            RevokedAt = DateTime.UtcNow;

            AddDomainEvent(
                new SessionRevokedDomainEvent(
                    Id,
                    UserId,
                    RevokedAt.Value));

            return Result.Success();
        }

        public Result RotateRefreshToken(RefreshTokenHash refreshTokenHash)
        {
            if (!IsActive())
                return Result.Failure(SessionErrors.SessionNotActive());

            if (refreshTokenHash is null)
                return Result.Failure(SessionErrors.RefreshTokenHashRequired());

            if (RefreshTokenHash == refreshTokenHash)
                return Result.Failure(SessionErrors.RefreshTokenAlreadyInUse());

            RefreshTokenHash = refreshTokenHash;

            LastActivityAt = DateTime.UtcNow;

            AddDomainEvent(
                new RefreshTokenRotatedDomainEvent(
                    Id,
                    UserId,
                    LastActivityAt.Value));

            return Result.Success();
        }

        public bool IsActive()
        {
            return !IsExpired() && !IsRevoked();
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow >= ExpiresAt;
        }

        public bool IsRevoked()
        {
            return RevokedAt.HasValue;
        }
    }
}