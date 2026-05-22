using Domain.Aggregates.Roles.Aggregate;

namespace Application.Abstractions.Persistence
{
    public interface IRoleRepository
    {
        Task AddAsync(
        Role role,
        CancellationToken cancellationToken = default);

        Task<Role?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}