using Application.Abstractions.Handlers;
using Application.Memberships.Commands.ActivateMembership;
using Application.Memberships.Commands.AssignRoleToMembership;
using Application.Memberships.Commands.CreateMembership;
using Application.Roles.Commands.ActivateRole;
using Application.Roles.Commands.AddPermissionToRole;
using Application.Roles.Commands.CreateRole;
using Application.Roles.Commands.DeactivateRole;
using Application.Roles.Commands.RemovePermissionFromRole;
using Application.Roles.Commands.RenameRole;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccessControlApplication(
            this IServiceCollection services)
        {
            // Roles
            services.AddScoped<ICreateRoleCommandHandler, CreateRoleCommandHandler>();
            services.AddScoped<IActivateRoleCommandHandler, ActivateRoleCommandHandler>();
            services.AddScoped<IAddPermissionToRoleCommandHandler, AddPermissionToRoleCommandHandler>();
            services.AddScoped<IDeactivateRoleCommandHandler, DeactivateRoleCommandHandler>();
            services.AddScoped<IRemovePermissionFromRoleCommandHandler, RemovePermissionFromRoleCommandHandler>();
            services.AddScoped<IRenameRoleCommandHandler, RenameRoleCommandHandler>();

            // Memberships
            services.AddScoped<IActivateMembershipCommandHandler, ActivateMembershipCommandHandler>();
            services.AddScoped<IAssignRoleToMembershipCommandHandler, AssignRoleToMembershipCommandHandler>();
            services.AddScoped<ICreateMembershipCommandHandler, CreateMembershipCommandHandler>();

            return services;
        }
    }
}