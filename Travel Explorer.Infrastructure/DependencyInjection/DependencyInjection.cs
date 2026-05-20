using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Travel_Explorer.Infrastructure.Data;
using Travel_Explorer.Infrastructure.Repositories;

namespace Travel_Explorer.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"), 
                    npgsqlOptions => {
                        
                        npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    }
                )
            );

            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtAuthService, JwtAuthService>();

            
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

           
            return services;
        }
    }
}