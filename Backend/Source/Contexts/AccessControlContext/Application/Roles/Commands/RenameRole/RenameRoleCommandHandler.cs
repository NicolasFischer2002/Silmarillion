using Application.Abstractions.Persistence;
using Domain.Aggregates.Roles.Errors;
using Domain.Aggregates.Roles.ValueObjects;
using SharedKernel.Results;

namespace Application.Roles.Commands.RenameRole
{
    public sealed class RenameRoleCommandHandler
    {
        private readonly IRoleRepository _roleRepository;

        private readonly IUnitOfWork _unitOfWork;

        public RenameRoleCommandHandler(
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            RenameRoleCommand command,
            CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository.GetByIdAsync(
                command.RoleId,
                cancellationToken);

            if (role is null)
                return Result.Failure(RoleErrors.RoleNotFound());

            var roleNameResult = RoleName.Create(command.Name);

            if (roleNameResult.IsFailure)
                return Result.Failure(roleNameResult.Errors);

            var renameResult = role.Rename(roleNameResult.Value);

            if (renameResult.IsFailure)
                return Result.Failure(renameResult.Errors);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}