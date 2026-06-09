using Domain.Aggregates.Roles.Constants;

namespace Application.Abstractions.Authorization
{
    public interface IPermissionEvaluator
    {
        Task<bool> HasPermissionAsync(
            Guid userId,
            Guid organizationId,
            PermissionCode permission,
            CancellationToken cancellationToken = default);
    }
}