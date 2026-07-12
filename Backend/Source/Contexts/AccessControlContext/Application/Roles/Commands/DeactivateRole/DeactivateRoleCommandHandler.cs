using Application.Abstractions.Handlers;
using Application.Abstractions.Persistence;
using Domain.Aggregates.Role.Errors;
using SharedKernel.Results;

namespace Application.Roles.Commands.DeactivateRole
{
    public sealed class DeactivateRoleCommandHandler : IDeactivateRoleCommandHandler
    {
        private readonly IRoleRepository _roleRepository;

        private readonly IUnitOfWork _unitOfWork;

        public DeactivateRoleCommandHandler(
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            DeactivateRoleCommand command,
            CancellationToken cancellationToken = default)
        {
            var role = await _roleRepository.GetByIdAsync(
                command.RoleId,
                cancellationToken);

            if (role is null)
                return Result.Failure(RoleErrors.RoleNotFound());

            var deactivateResult = role.Deactivate();

            if (deactivateResult.IsFailure)
                return Result.Failure(deactivateResult.Errors);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}