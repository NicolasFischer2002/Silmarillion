using Domain.Aggregates.Membership.Aggregate;

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

        Task<Membership?> GetByUserAndOrganizationAsync(
            Guid userId,
            Guid organizationId,
            CancellationToken cancellationToken = default);
    }
}