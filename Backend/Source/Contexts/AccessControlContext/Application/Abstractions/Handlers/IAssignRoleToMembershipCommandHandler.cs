using Application.Memberships.Commands.AssignRoleToMembership;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface IAssignRoleToMembershipCommandHandler
    {
        Task<Result> HandleAsync(
            AssignRoleToMembershipCommand command,
            CancellationToken cancellationToken = default);
    }
}