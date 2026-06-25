using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Application.Common.Interfaces;
using Polly;
using Travel_Explorer.Infrastructure.Data;
using Travel_Explorer.Infrastructure.Repositories;
using Travel_Explorer.Infrastructure.Services;
using Travel_Explorer.Application.Services.Payment;

namespace Travel_Explorer.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? configuration["ConnectionStrings__DefaultConnection"];
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = configuration["POSTGRESQLCONNSTR_DefaultConnection"] ?? 
                                   Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_DefaultConnection");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {

                        npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    }
                )
            );


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

            //  الحل الآمن لقراءة وحقن إعدادات Cloudinary
            services.Configure<CloudinarySettings>(options =>
            {
                options.CloudName = configuration["Cloudinary:CloudName"] ?? configuration["Cloudinary__CloudName"];
                options.ApiKey = configuration["Cloudinary:ApiKey"] ?? configuration["Cloudinary__ApiKey"];
                options.ApiSecret = configuration["Cloudinary:ApiSecret"] ?? configuration["Cloudinary__ApiSecret"];
            });
            
            services.AddScoped<IPhotoService, CloudinaryPhotoService>();

            //  الحل السحري والقاطع لإصلاح مشكلة الـ Paymob ومنع الـ ValidateOnStart من التسبب في انهيار التطبيق
            services.Configure<PaymobtSettings>(options =>
            {
                options.ApiKey = configuration["PaymobSettings:ApiKey"] ?? configuration["PaymobSettings__ApiKey"];
                options.PublicKey = configuration["PaymobSettings:PublicKey"] ?? configuration["PaymobSettings__PublicKey"];
                options.IFrameId = configuration["PaymobSettings:IFrameId"] ?? configuration["PaymobSettings__IFrameId"];
                options.HmacSecret = configuration["PaymobSettings:HmacSecret"] ?? configuration["PaymobSettings__HmacSecret"];
                options.Currency = configuration["PaymobSettings:Currency"] ?? configuration["PaymobSettings__Currency"];
                
                // إسناد القيمة مباشرة كـ string بدون محاولة تحويلها لـ int
                options.PaymentMethodId = configuration["PaymobSettings:PaymentMethodId"] ?? configuration["PaymobSettings__PaymentMethodId"];
            });

            // إعداد الـ HttpClient الخاص بـ Paymob بشكل سليم
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

            services.AddScoped<IFlightSchedualRepository, FlightSchedualRepository>();

            return services;
        }
    }
}
