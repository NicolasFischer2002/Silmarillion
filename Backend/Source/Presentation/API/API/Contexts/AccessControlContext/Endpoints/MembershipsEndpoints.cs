using API.Common;
using API.Contexts.AccessControlContext.Contracts.Requests;
using Application.Abstractions.Handlers;
using Application.Memberships.Commands.ActivateMembership;
using Application.Memberships.Commands.AssignRoleToMembership;
using Application.Memberships.Commands.CreateMembership;

namespace API.Contexts.AccessControlContext.Endpoints
{
    public static class MembershipsEndpoints
    {
        public static void Map(RouteGroupBuilder group)
        {
            var memberships = group.MapGroup("/memberships");

            memberships.MapPatch("/{membershipId:guid}/activate", ActivateMembership)
                .WithName("ActivateMembership")
                .WithTags("Memberships")
                .Produces(StatusCodes.Status204NoContent);

            memberships.MapPost("/{membershipId:guid}/assign-role", AssignRoleToMembership)
                .WithName("AssignRoleToMembership")
                .WithTags("Memberships")
                .Produces(StatusCodes.Status204NoContent);

            memberships.MapPost("/", CreateMembership)
                .WithName("CreateMembership")
                .WithTags("Memberships")
                .Produces<CreateMembershipResponse>(StatusCodes.Status201Created);
        }

        private static async Task<IResult> ActivateMembership(
            Guid membershipId,
            IActivateMembershipCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new ActivateMembershipCommand(membershipId);

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
                return ErrorMapper.Map([.. result.Errors]);

            return Results.NoContent();
        }

        private static async Task<IResult> AssignRoleToMembership(
            Guid membershipId,
            AssignRoleToMembershipRequest request,
            IAssignRoleToMembershipCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new AssignRoleToMembershipCommand(
                membershipId,
                request.RoleId);

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
                return ErrorMapper.Map([.. result.Errors]);

            return Results.NoContent();
        }

        private static async Task<IResult> CreateMembership(
            CreateMembershipRequest request,
            ICreateMembershipCommandHandler handler,
            CancellationToken cancellationToken)
        {
            var command = new CreateMembershipCommand(
                request.UserId,
                request.OrganizationId);

            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsFailure)
                return ErrorMapper.Map([.. result.Errors]);

            return Results.Created(
                $"/api/access-control/memberships/{result.Value.MembershipId}",
                result.Value);
        }
    }
}