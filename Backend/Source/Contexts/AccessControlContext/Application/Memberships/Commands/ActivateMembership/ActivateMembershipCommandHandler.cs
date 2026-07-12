using Application.Abstractions.Handlers;
using Application.Abstractions.Persistence;
using Domain.Aggregates.Membership.Errors;
using SharedKernel.Results;

namespace Application.Memberships.Commands.ActivateMembership
{
    public sealed class ActivateMembershipCommandHandler : IActivateMembershipCommandHandler
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateMembershipCommandHandler(
            IMembershipRepository membershipRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            ActivateMembershipCommand command,
            CancellationToken cancellationToken = default)
        {
            var membership = await _membershipRepository.GetByIdAsync(
                command.MembershipId,
                cancellationToken);

            if (membership is null)
                return Result.Failure(MembershipErrors.MembershipNotFound());

            var activateResult = membership.Activate();

            if (activateResult.IsFailure)
                return Result.Failure(activateResult.Errors);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}