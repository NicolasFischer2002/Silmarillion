using Application.Abstractions.Persistence;
using Domain.Aggregates.Roles.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Roles
{
    public sealed class RoleRepository : IRoleRepository
    {
        private readonly AccessControlDbContext _dbContext;

        public RoleRepository(AccessControlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(
            Role role,
            CancellationToken cancellationToken)
        {
            await _dbContext.Roles.AddAsync(
                role,
                cancellationToken);
        }

        public async Task<Role?> GetByIdAsync(
            Guid roleId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(
                    role => role.Id == roleId,
                    cancellationToken);
        }

        public Task<IReadOnlyCollection<Role>> GetByIdsAsync(
            IReadOnlyCollection<Guid> roleIds, 
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}