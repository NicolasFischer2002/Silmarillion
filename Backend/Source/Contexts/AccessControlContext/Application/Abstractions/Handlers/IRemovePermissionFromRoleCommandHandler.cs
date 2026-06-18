using Application.Roles.Commands.RemovePermissionFromRole;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface IRemovePermissionFromRoleCommandHandler
    {
        Task<Result> HandleAsync(
            RemovePermissionFromRoleCommand command,
            CancellationToken cancellationToken = default);
    }
}