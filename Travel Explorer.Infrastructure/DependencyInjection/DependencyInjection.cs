using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Application.Common.Interfaces;
using Travel_Explorer.Infrastructure.Data;
using Travel_Explorer.Infrastructure.Repositories;
using Travel_Explorer.Infrastructure.Services;

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

            // Register Identity Services
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();



            services.AddScoped<IUnitOfWork, UnitOfWork>();

            
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Cloudinary Photo Upload
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            services.AddScoped<IPhotoService, CloudinaryPhotoService>();

            return services;
        }
    }
}