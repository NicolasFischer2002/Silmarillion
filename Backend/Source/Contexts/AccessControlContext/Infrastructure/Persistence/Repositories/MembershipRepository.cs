using Application.Abstractions.Persistence;
using Domain.Aggregates.Memberships.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class MembershipRepository: IMembershipRepository
    {
        private readonly AccessControlDbContext _dbContext;

        public MembershipRepository(AccessControlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(
            Membership membership,
            CancellationToken cancellationToken)
        {
            await _dbContext.Memberships.AddAsync(
                membership,
                cancellationToken);
        }

        public async Task<Membership?> GetByIdAsync(
            Guid membershipId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Memberships
                .FirstOrDefaultAsync(
                    membership => membership.Id == membershipId,
                    cancellationToken);
        }

        public Task<Membership?> GetByUserAndOrganizationAsync(
            Guid userId, 
            Guid organizationId, 
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}