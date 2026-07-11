using Application.Memberships.Commands.CreateMembership;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface ICreateMembershipCommandHandler
    {
        Task<Result<CreateMembershipResponse>> HandleAsync(
            CreateMembershipCommand command,
            CancellationToken cancellationToken = default);
    }
}