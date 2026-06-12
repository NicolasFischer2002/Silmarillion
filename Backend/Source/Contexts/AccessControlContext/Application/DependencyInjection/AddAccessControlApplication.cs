using Application.Abstractions.Handlers;
using Application.Roles.Commands.ActivateRole;
using Application.Roles.Commands.CreateRole;
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

            return services;
        }
    }
}