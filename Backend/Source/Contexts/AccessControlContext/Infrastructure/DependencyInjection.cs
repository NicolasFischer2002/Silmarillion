using Application.Abstractions.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories.Memberships;
using Infrastructure.Persistence.Repositories.Roles;
using Infrastructure.Persistence.Repositories.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccessControlInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AccessControl");

            services.AddDbContext<AccessControlDbContext>(
                options =>
                {
                    options.UseNpgsql(connectionString);
                });


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            return services;
        }
    }
}