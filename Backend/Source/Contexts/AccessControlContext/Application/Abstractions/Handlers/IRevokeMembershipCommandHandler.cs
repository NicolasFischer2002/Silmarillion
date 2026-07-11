using Application.Memberships.Commands.RevokeMembership;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface IRevokeMembershipCommandHandler
    {
        Task<Result> HandleAsync(
            RevokeMembershipCommand command,
            CancellationToken cancellationToken = default);
    }
}