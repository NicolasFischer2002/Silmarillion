using Application.Abstractions.Handlers;
using Application.Abstractions.Persistence;
using Domain.Aggregates.Memberships.Aggregate;
using SharedKernel.Results;

namespace Application.Memberships.Commands.CreateMembership
{
    public sealed class CreateMembershipCommandHandler : ICreateMembershipCommandHandler
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMembershipCommandHandler(
            IMembershipRepository membershipRepository,
            IUnitOfWork unitOfWork)
        {
            _membershipRepository = membershipRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateMembershipResponse>> HandleAsync(
            CreateMembershipCommand command,
            CancellationToken cancellationToken = default)
        {
            var membershipResult =
                Membership.CreatePending(
                    Guid.NewGuid(),
                    command.UserId,
                    command.OrganizationId);

            if (membershipResult.IsFailure)
                return Result<CreateMembershipResponse>.Failure(membershipResult.Errors);

            await _membershipRepository.AddAsync(
                membershipResult.Value,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<CreateMembershipResponse>
                .Success(
                    new CreateMembershipResponse(
                        membershipResult.Value.Id,
                        membershipResult.Value.UserId,
                        membershipResult.Value.OrganizationId));
        }
    }
}