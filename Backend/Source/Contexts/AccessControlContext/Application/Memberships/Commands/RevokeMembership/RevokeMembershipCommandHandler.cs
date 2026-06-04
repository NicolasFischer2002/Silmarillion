using Application.Abstractions.Persistence;
using Domain.Aggregates.Memberships.Errors;
using SharedKernel.Results;

namespace Application.Memberships.Commands.RevokeMembership
{
    public sealed class RevokeMembershipCommandHandler
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RevokeMembershipCommandHandler(
            IMembershipRepository membershipRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            RevokeMembershipCommand command,
            CancellationToken cancellationToken = default)
        {
            var membership = await _membershipRepository.GetByIdAsync(
                command.MembershipId,
                cancellationToken);

            if (membership is null)
                return Result.Failure(MembershipErrors.MembershipNotFound());

            var revokeResult = membership.Revoke();

            if (revokeResult.IsFailure)
                return Result.Failure(revokeResult.Errors);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}