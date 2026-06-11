using Application.Abstractions.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("AccessControl");

            services.AddDbContext<Persistence.AccessControlDbContext>(
                options =>
                {
                    options.UseNpgsql(connectionString);
                });


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            return services;
        }
    }
}