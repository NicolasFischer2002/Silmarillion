using Application.Roles.Commands.CreateRole;
using SharedKernel.Results;

namespace Application.Abstractions.Handlers
{
    public interface ICreateRoleCommandHandler
    {
        Task<Result<CreateRoleResponse>> HandleAsync(
            CreateRoleCommand command,
            CancellationToken cancellationToken = default);
    }
}