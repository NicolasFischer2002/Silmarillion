using API.Contexts.AccessControlContext.Endpoints;
using Application.DependencyInjection;
using Infrastructure;

namespace API.Contexts.AccessControlContext
{
    public static class AccessControlModule
    {
        public static IServiceCollection AddAccessControlModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAccessControlApplication();
            services.AddAccessControlInfrastructure(configuration);

            return services;
        }

        public static IEndpointRouteBuilder MapAccessControlModule(
            this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/access-control");

            RolesEndpoints.Map(group);

            return app;
        }
    }
}