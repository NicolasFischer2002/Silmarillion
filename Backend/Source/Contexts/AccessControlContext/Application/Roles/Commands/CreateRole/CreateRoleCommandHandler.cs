using Application.Abstractions.Handlers;
using Application.Abstractions.Persistence;
using Domain.Aggregates.Roles.Aggregate;
using Domain.Aggregates.Roles.ValueObjects;
using SharedKernel.Results;

namespace Application.Roles.Commands.CreateRole
{
    public sealed class CreateRoleCommandHandler : ICreateRoleCommandHandler
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoleCommandHandler(
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateRoleResponse>> HandleAsync(
            CreateRoleCommand command,
            CancellationToken cancellationToken = default)
        {
            var roleNameResult = RoleName.Create(command.Name);

            if (roleNameResult.IsFailure)
                return Result<CreateRoleResponse>.Failure(roleNameResult.Errors);

            var roleResult = Role.Create(
                Guid.NewGuid(),
                command.OrganizationId,
                roleNameResult.Value);

            if (roleResult.IsFailure)
                return Result<CreateRoleResponse>.Failure(roleResult.Errors);

            await _roleRepository.AddAsync(roleResult.Value, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<CreateRoleResponse>.Success(
                new CreateRoleResponse(
                    roleResult.Value.Id,
                    roleResult.Value.Name.ToString()));
        }
    }
}