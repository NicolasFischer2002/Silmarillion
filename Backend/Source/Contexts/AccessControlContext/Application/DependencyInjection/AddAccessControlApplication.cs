using Application.Roles.Commands.CreateRole;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccessControlApplication(
            this IServiceCollection services)
        {
            services.AddScoped<CreateRoleCommandHandler>();

            return services;
        }
    }
}