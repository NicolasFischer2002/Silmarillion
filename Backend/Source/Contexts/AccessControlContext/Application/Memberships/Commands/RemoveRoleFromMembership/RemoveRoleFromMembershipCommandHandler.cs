using Application.Abstractions.Handlers;
using Application.Abstractions.Persistence;
using Domain.Aggregates.Membership.Errors;
using SharedKernel.Results;

namespace Application.Memberships.Commands.RemoveRoleFromMembership
{
    public sealed class RemoveRoleFromMembershipCommandHandler : IRemoveRoleFromMembershipCommandHandler
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveRoleFromMembershipCommandHandler(
            IMembershipRepository membershipRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            RemoveRoleFromMembershipCommand command,
            CancellationToken cancellationToken = default)
        {
            var membership = await _membershipRepository.GetByIdAsync(
                command.MembershipId,
                cancellationToken);

            if (membership is null)
                return Result.Failure(MembershipErrors.MembershipNotFound());

            var removeResult = membership.RemoveRole(command.RoleId);

            if (removeResult.IsFailure)
                return Result.Failure(removeResult.Errors);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}