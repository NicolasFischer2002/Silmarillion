using Application.Abstractions.Persistence;
using Domain.Aggregates.Membership.Constants;
using Domain.Aggregates.Roles.Constants;

namespace Application.Permissions.Queries.GetEffectivePermissions
{
    public sealed class GetEffectivePermissionsQueryHandler
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IRoleRepository _roleRepository;

        public GetEffectivePermissionsQueryHandler(
            IMembershipRepository membershipRepository,
            IRoleRepository roleRepository)
        {
            _membershipRepository = membershipRepository;
            _roleRepository = roleRepository;
        }

        public async Task<GetEffectivePermissionsResponse> HandleAsync(
            GetEffectivePermissionsQuery query,
            CancellationToken cancellationToken = default)
        {
            var membership = await _membershipRepository.GetByUserAndOrganizationAsync(
                query.UserId,
                query.OrganizationId,
                cancellationToken);

            if (membership is null)
                return new GetEffectivePermissionsResponse([]);

            if (membership.Status != MembershipStatus.Active)
                return new GetEffectivePermissionsResponse([]);

            var roles = await _roleRepository.GetByIdsAsync(
                [.. membership.AssignedRoles.Values],
                cancellationToken);

            var permissions = roles
                .Where(role => role.Status == RoleStatus.Active)
                .SelectMany(role => role.Permissions.Values)
                .Distinct()
                .ToList();

            return new GetEffectivePermissionsResponse(permissions);
        }
    }
}