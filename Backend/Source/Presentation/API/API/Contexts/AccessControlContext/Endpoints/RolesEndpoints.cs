using API.Common;
using API.Contexts.AccessControlContext.Contracts.Requests;
using Application.Abstractions.Handlers;
using Application.Roles.Commands.ActivateRole;
using Application.Roles.Commands.CreateRole;

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
    }
}