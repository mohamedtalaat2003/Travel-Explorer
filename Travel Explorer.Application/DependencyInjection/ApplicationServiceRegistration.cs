using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Behaviors;

namespace Travel_Explorer.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationServiceRegistration).Assembly;
           
            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UserBlockBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });
            
            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
