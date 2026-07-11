using Application.Memberships.Commands.DeactivateMembership;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface IDeactivateMembershipCommandHandler
    {
        Task<Result> HandleAsync(
            DeactivateMembershipCommand command,
            CancellationToken cancellationToken = default);
    }
}