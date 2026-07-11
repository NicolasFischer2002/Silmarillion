using Application.Memberships.Commands.RemoveRoleFromMembership;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface IRemoveRoleFromMembershipCommandHandler
    {
        Task<Result> HandleAsync(
            RemoveRoleFromMembershipCommand command,
            CancellationToken cancellationToken = default);
    }
}