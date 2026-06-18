using Application.Roles.Commands.AddPermissionToRole;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface IAddPermissionToRoleCommandHandler
    {
        Task<Result> HandleAsync(
            AddPermissionToRoleCommand command,
            CancellationToken cancellationToken = default);
    }
}