using Domain.Aggregates.Memberships.Aggregate;

namespace Application.Abstractions.Persistence
{
    public interface IMembershipRepository
    {
        Task AddAsync(
        Membership membership,
        CancellationToken cancellationToken = default);

        Task<Membership?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}