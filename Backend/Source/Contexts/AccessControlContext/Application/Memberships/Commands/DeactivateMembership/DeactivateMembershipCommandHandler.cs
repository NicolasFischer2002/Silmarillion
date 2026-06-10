using Application.Abstractions.Persistence;
using Domain.Aggregates.Memberships.Errors;
using SharedKernel.Results;

namespace Application.Memberships.Commands.DeactivateMembership
{
    public sealed class DeactivateMembershipCommandHandler
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateMembershipCommandHandler(
            IMembershipRepository membershipRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            DeactivateMembershipCommand command,
            CancellationToken cancellationToken = default)
        {
            var membership = await _membershipRepository.GetByIdAsync(
                command.MembershipId,
                cancellationToken);

            if (membership is null)
                return Result.Failure(MembershipErrors.MembershipNotFound());

            var deactivateResult = membership.Deactivate();

            if (deactivateResult.IsFailure)
                return Result.Failure(deactivateResult.Errors);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}