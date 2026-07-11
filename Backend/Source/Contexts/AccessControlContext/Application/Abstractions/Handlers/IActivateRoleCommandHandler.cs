using Application.Roles.Commands.ActivateRole;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface IActivateRoleCommandHandler
    {
        Task<Result> HandleAsync(
            ActivateRoleCommand command,
            CancellationToken cancellationToken = default);
    }
}