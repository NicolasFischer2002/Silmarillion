using Application.Abstractions.Persistence;
using Domain.Aggregates.Memberships.Errors;
using Domain.Aggregates.Roles.Errors;
using Domain.Errors;
using SharedKernel.Results;

namespace Application.Memberships.Commands.AssignRoleToMembership
{
    public sealed class AssignRoleToMembershipCommandHandler
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignRoleToMembershipCommandHandler(
            IMembershipRepository membershipRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            AssignRoleToMembershipCommand command,
            CancellationToken cancellationToken = default)
        {
            var membership = await _membershipRepository.GetByIdAsync(
                command.MembershipId,
                cancellationToken);

            if (membership is null)
                return Result.Failure(MembershipErrors.MembershipNotFound());

            var role = await _roleRepository.GetByIdAsync(
                command.RoleId,
                cancellationToken);

            if (role is null)
                return Result.Failure(RoleErrors.RoleNotFound());

            if (membership.OrganizationId != role.OrganizationId)
                return Result.Failure(AccessControlErrors.RoleDoesNotBelongToOrganization());

            var assignResult = membership.AssignRole(role.Id);

            if (assignResult.IsFailure)
                return Result.Failure(assignResult.Errors);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}