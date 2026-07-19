using Domain.Aggregates.Session.Aggregate;
using Domain.Aggregates.Session.ValueObjects;

namespace Application.Abstractions.Persistence
{
    public interface ISessionRepository
    {
        Task AddAsync(
            Session session,
            CancellationToken cancellationToken);

        Task<Session?> GetByIdAsync(
            Guid sessionId,
            CancellationToken cancellationToken);

        Task<Session?> GetByRefreshTokenHashAsync(
            RefreshTokenHash refreshTokenHash,
            CancellationToken cancellationToken);

        Task<IReadOnlyCollection<Session>> GetActiveSessionsByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken);
    }
}