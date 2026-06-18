using Application.Abstractions.Handlers;
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
            services.AddScoped<ICreateRoleCommandHandler, CreateRoleCommandHandler>();
            services.AddScoped<IActivateRoleCommandHandler, ActivateRoleCommandHandler>();
            services.AddScoped<IAddPermissionToRoleCommandHandler, AddPermissionToRoleCommandHandler>();
            services.AddScoped<IDeactivateRoleCommandHandler, DeactivateRoleCommandHandler>();
            services.AddScoped<IRemovePermissionFromRoleCommandHandler, RemovePermissionFromRoleCommandHandler>();
            services.AddScoped<IRenameRoleCommandHandler, RenameRoleCommandHandler>();

            return services;
        }
    }
}