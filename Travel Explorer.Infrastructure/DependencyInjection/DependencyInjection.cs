using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Travel_Explorer.Application.Common.Interfaces;
using Polly;
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
            services.AddScoped<IJwtAuthService, JwtAuthService>();

            
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Cloudinary Photo Upload
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            services.AddScoped<IPhotoService, CloudinaryPhotoService>();

            // Payment Strategy & Factory
            services.AddOptions<Travel_Explorer.Application.Services.Payment.PaymobtSettings>()
                .BindConfiguration("PaymobSettings")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddHttpClient<Travel_Explorer.Infrastructure.Services.Payment.PaymobGateway>()
                .AddPolicyHandler(Polly.Extensions.Http.HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(3, a => System.TimeSpan.FromSeconds(System.Math.Pow(2, a))))
                .AddPolicyHandler(Polly.Extensions.Http.HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(5, System.TimeSpan.FromSeconds(30)));

            services.AddScoped<Travel_Explorer.Application.Services.Payment.IPaymentGateway>(sp => 
                sp.GetRequiredService<Travel_Explorer.Infrastructure.Services.Payment.PaymobGateway>());
            services.AddScoped<Travel_Explorer.Application.Services.Payment.IPaymentGatewayFactory, 
                Travel_Explorer.Infrastructure.Services.Payment.PaymentGatewayFactory>();

            return services;
        }
    }
}