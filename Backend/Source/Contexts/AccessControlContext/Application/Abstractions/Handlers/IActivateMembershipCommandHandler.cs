using Application.Memberships.Commands.ActivateMembership;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface IActivateMembershipCommandHandler
    {
        Task<Result> HandleAsync(
            ActivateMembershipCommand command,
            CancellationToken cancellationToken = default);
    }
}