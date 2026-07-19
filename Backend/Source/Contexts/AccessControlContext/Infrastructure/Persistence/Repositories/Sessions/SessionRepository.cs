using Application.Abstractions.Persistence;
using Domain.Aggregates.Session.Aggregate;
using Domain.Aggregates.Session.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Sessions
{
    public sealed class SessionRepository : ISessionRepository
    {
        private readonly AccessControlDbContext _dbContext;

        public SessionRepository(
            AccessControlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(
            Session session,
            CancellationToken cancellationToken)
        {
            await _dbContext.Sessions.AddAsync(
                session,
                cancellationToken);
        }

        public async Task<Session?> GetByIdAsync(
            Guid sessionId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Sessions
                .FirstOrDefaultAsync(
                    session => session.Id == sessionId,
                    cancellationToken);
        }

        public async Task<Session?> GetByRefreshTokenHashAsync(
            RefreshTokenHash refreshTokenHash,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Sessions
                .FirstOrDefaultAsync(
                    session => session.RefreshTokenHash == refreshTokenHash,
                    cancellationToken);
        }

        public async Task<IReadOnlyCollection<Session>> GetActiveSessionsByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Sessions
                .Where(session =>
                    session.UserId == userId &&
                    session.RevokedAt == null &&
                    session.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(cancellationToken);
        }
    }
}