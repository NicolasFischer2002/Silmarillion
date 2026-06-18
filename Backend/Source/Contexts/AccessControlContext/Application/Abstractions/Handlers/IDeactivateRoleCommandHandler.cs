using Application.Roles.Commands.DeactivateRole;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface IDeactivateRoleCommandHandler
    {
        Task<Result> HandleAsync(
            DeactivateRoleCommand command,
            CancellationToken cancellationToken = default);
    }
}