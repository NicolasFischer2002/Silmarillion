using Application.Abstractions.Handlers;
using Application.Abstractions.Persistence;
using Domain.Aggregates.Roles.Errors;
using SharedKernel.Results;

namespace Application.Roles.Commands.AddPermissionToRole
{
    public sealed class AddPermissionToRoleCommandHandler : IAddPermissionToRoleCommandHandler
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddPermissionToRoleCommandHandler(
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            AddPermissionToRoleCommand command,
            CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository.GetByIdAsync(
                command.RoleId,
                cancellationToken);

            if (role is null)
                return Result.Failure(RoleErrors.RoleNotFound());

            var addPermissionResult =
                role.AddPermission(command.Permission);

            if (addPermissionResult.IsFailure)
                return Result.Failure(addPermissionResult.Errors);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}