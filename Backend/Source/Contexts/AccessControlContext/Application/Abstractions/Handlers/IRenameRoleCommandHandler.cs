using Application.Roles.Commands.RenameRole;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface IRenameRoleCommandHandler
    {
        Task<Result> HandleAsync(
            RenameRoleCommand command,
            CancellationToken cancellationToken = default);
    }
}