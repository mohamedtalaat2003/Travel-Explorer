
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Travel_Explorer.Application.DependencyInjection;
using Travel_Explorer.Application.Services;
using Travel_Explorer.Application.Services.Payment;
using Travel_Explorer.Infrastructure.DependencyInjection;
using Travel_Explorer.Infrastructure.Data;
using Travel_Explorer.Middleware;
using Microsoft.AspNetCore.Identity;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Infrastructure.Persistence.Seed;

namespace Travel_Explorer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll", policy => {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddHttpContextAccessor();
          
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Travel Explorer API", Version = "v1" });
                
                // Add JWT Security Definition
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Register CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Authentication & Authorization
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();

            if (string.IsNullOrEmpty(jwtSettings.Token))
            {
                jwtSettings.Token = "ThisIsAVeryLongAndSuperSecureSecretKeyThatIsAtLeast32BytesLongaslhafkafna;f;230982050345afba!!!!";
            }
            if (string.IsNullOrEmpty(jwtSettings.Issuer))
            {
                jwtSettings.Issuer = "http://travelexplorer.somee.com";
            }
            if (string.IsNullOrEmpty(jwtSettings.Audience))
            {
                jwtSettings.Audience = "MyAwesomeAudience";
            }

            builder.Services.Configure<PaymobtSettings>(builder.Configuration.GetSection("PaymobSettings"));

            var authBuilder = builder.Services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddCookie("ExternalCookie") //temp cookie for google schema
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Token))
                    };
                });

            if (!string.IsNullOrEmpty(jwtSettings.GoogleClientId) && !string.IsNullOrEmpty(jwtSettings.GoogleClientSecret))
            {
                authBuilder.AddGoogle(options =>
                {
                    options.ClientId = jwtSettings.GoogleClientId;
                    options.ClientSecret = jwtSettings.GoogleClientSecret;
                    options.SignInScheme = "ExternalCookie";
                });
            }


         

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseMiddleware<Middleware.ExceptionMiddleware>();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("AllowAll");

            
            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");
            
            app.UsePaymentWebhookVerification();
            app.MapControllers();

            // Seed Roles & Data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
                    RoleSeeder.SeedRolesAsync(roleManager).GetAwaiter().GetResult();

                    // Seed Data (Categories, Destinations, Activities, Flights, Messages)
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();
                    DataSeeder.SeedDataAsync(dbContext).GetAwaiter().GetResult();

                    // Seed Users (2 Admin, 5 Traveler, 3 Author)
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    DataSeeder.SeedUsersAsync(userManager).GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    // Handle or log error
                }
            }

            app.Run();
        }
    }
}
