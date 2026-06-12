using API.Common;
using API.Contexts.AccessControlContext.Contracts.Requests;
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
                .Produces<Guid>(StatusCodes.Status201Created);
        }

        private static async Task<IResult> CreateRole(
            CreateRoleRequest request,
            CreateRoleCommandHandler handler,
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
    }
}