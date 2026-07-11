using API.Common;
using API.Contexts.AccessControlContext.Contracts.Requests;
using Application.Abstractions.Handlers;
using Application.Roles.Commands.ActivateRole;
using Application.Roles.Commands.CreateRole;
using Application.Roles.Commands.DeactivateRole;
using Application.Roles.Commands.RemovePermissionFromRole;
using Application.Roles.Commands.RenameRole;

namespace API.Contexts.AccessControlContext.Endpoints
{
    public static class RolesEndpoints
    {
        public static void Map(RouteGroupBuilder group)
        {
            var roles = group.MapGroup("/roles");

            roles.MapPost("/", CreateRole)
                .WithName("CreateRole")
                .WithTags("Roles")
                .Produces<CreateRoleResponse>(StatusCodes.Status201Created);

            roles.MapPatch("/{roleId:guid}/activate", ActivateRole)
                .WithName("ActivateRole")
                .WithTags("Roles")
                .Produces(StatusCodes.Status204NoContent);

            roles.MapPost("/{roleId:guid}/permissions", AddPermissionToRole)
                .WithName("AddPermissionToRole")
                .WithTags("Roles")
                .Produces(StatusCodes.Status204NoContent);

            roles.MapPost("/{roleId:guid}/deactivate", DeactivateRole)
                .WithName("DeactivateRole")
                .WithTags("Roles")
                .Produces(StatusCodes.Status204NoContent);

            roles.MapDelete("/{roleId:guid}/permissions", RemovePermissionFromRole)
                .WithName("RemovePermissionFromRole")
                .WithTags("Roles")
                .Produces(StatusCodes.Status204NoContent);

            roles.MapPatch("/{roleId:guid}/rename", RenameRole)
                .WithName("RenameRole")
                .WithTags("Roles")
                .Produces(StatusCodes.Status204NoContent);
        }

        private static async Task<IResult> CreateRole(
            CreateRoleRequest request,
            ICreateRoleCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new CreateRoleCommand(
                request.OrganizationId,
                request.Name);

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
                return ErrorMapper.Map([.. result.Errors]);

            return Results.Created(
                $"/api/access-control/roles/{result.Value.Id}",
                result.Value);
        }

        private static async Task<IResult> ActivateRole(
            Guid roleId,
            IActivateRoleCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new ActivateRoleCommand(roleId);

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
                return ErrorMapper.Map([.. result.Errors]);

            return Results.NoContent();
        }

        private static async Task<IResult> AddPermissionToRole(
            Guid roleId,
            AddPermissionToRoleRequest request,
            IAddPermissionToRoleCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new Application.Roles.Commands.AddPermissionToRole.AddPermissionToRoleCommand(
                roleId,
                request.Permission);

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
                return ErrorMapper.Map([.. result.Errors]);

            return Results.NoContent();
        }

        private static async Task<IResult> DeactivateRole(
            Guid roleId,
            IDeactivateRoleCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new DeactivateRoleCommand(roleId);

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
                return ErrorMapper.Map([.. result.Errors]);

            return Results.NoContent();
        }

        private static async Task<IResult> RemovePermissionFromRole(
            Guid roleId,
            RemovePermissionFromRoleRequest request,
            IRemovePermissionFromRoleCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new RemovePermissionFromRoleCommand(
                roleId,
                request.Permission);

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
                return ErrorMapper.Map([.. result.Errors]);

            return Results.NoContent();
        }

        private static async Task<IResult> RenameRole(
            Guid roleId,
            RenameRoleRequest request,
            IRenameRoleCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new RenameRoleCommand(
                roleId,
                request.NewName);

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
                return ErrorMapper.Map([.. result.Errors]);

            return Results.NoContent();
        }
    }
}