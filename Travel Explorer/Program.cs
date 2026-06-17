using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Travel_Explorer.Application.DependencyInjection;
using Travel_Explorer.Application.Services;
using Travel_Explorer.Application.Services.Payment;
using Travel_Explorer.Infrastructure.Data;
using Travel_Explorer.Infrastructure.DependencyInjection;
using Travel_Explorer.Infrastructure.Persistence.Seed;
using Travel_Explorer.Middleware;

namespace Travel_Explorer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("X-Pagination")
                          .SetPreflightMaxAge(TimeSpan.FromHours(1));
                });
            });

            //  الحل القاطع لقراءة الـ Connection String بجميع الصيغ المحتملة من Azure (بما فيها بادئة الـ PostgreSQL) أو اللوكال
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                                  ?? builder.Configuration["ConnectionStrings__DefaultConnection"]
                                  ?? builder.Configuration["POSTGRESQLCONNSTR_DefaultConnection"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Critical Error: Database Connection String is completely missing from configuration settings!");
            }

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.CommandTimeout(60); 
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorCodesToAdd: null);
                    }));

            //  قراءة بيانات الـ JWT والـ Google بشكل مباشر وصريح
            var jwtToken = builder.Configuration["JwtSettings:Token"] ?? builder.Configuration["JwtSettings__Token"];
            var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? builder.Configuration["JwtSettings__Issuer"];
            var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? builder.Configuration["JwtSettings__Audience"];
            
            var googleClientId = builder.Configuration["JwtSettings:GoogleClientId"] ?? builder.Configuration["JwtSettings__GoogleClientId"];
            var googleClientSecret = builder.Configuration["JwtSettings:GoogleClientSecret"] ?? builder.Configuration["JwtSettings__GoogleClientSecret"];

            if (string.IsNullOrEmpty(jwtToken))
            {
                throw new Exception("Critical Error: 'JwtSettings:Token' is totally missing from configuration!");
            }

            // حقن كائن الـ JwtSettings يدوياً
            var jwtSettings = new JwtSettings
            {
                Token = jwtToken,
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                AccessTokenExpirationMinutes = int.TryParse(builder.Configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? builder.Configuration["JwtSettings__AccessTokenExpirationMinutes"], out var accessMins) ? accessMins : 60,
                RefreshTokenExpirationDays = int.TryParse(builder.Configuration["JwtSettings:RefreshTokenExpirationDays"] ?? builder.Configuration["JwtSettings__RefreshTokenExpirationDays"], out var refreshDays) ? refreshDays : 7,
                GoogleClientId = googleClientId,
                GoogleClientSecret = googleClientSecret,
                GoogleFrontendRedirectURl = builder.Configuration["JwtSettings:GoogleFrontendRedirectUri"] ?? builder.Configuration["JwtSettings__GoogleFrontendRedirectUri"] ?? builder.Configuration["JwtSettings:GoogleFrontendRedirectUrl"] ?? builder.Configuration["JwtSettings__GoogleFrontendRedirectUrl"],
                GoogleFrontendloginRedirectUrl = builder.Configuration["JwtSettings:GoogleFrontendloginRedirectUrl"] ?? builder.Configuration["JwtSettings__GoogleFrontendloginRedirectUrl"]
            };

            builder.Services.Configure<JwtSettings>(options =>
            {
                options.Token = jwtSettings.Token;
                options.Issuer = jwtSettings.Issuer;
                options.Audience = jwtSettings.Audience;
                options.AccessTokenExpirationMinutes = jwtSettings.AccessTokenExpirationMinutes;
                options.RefreshTokenExpirationDays = jwtSettings.RefreshTokenExpirationDays;
                options.GoogleClientId = jwtSettings.GoogleClientId;
                options.GoogleClientSecret = jwtSettings.GoogleClientSecret;
                options.GoogleFrontendRedirectURl = jwtSettings.GoogleFrontendRedirectURl;
                options.GoogleFrontendloginRedirectUrl = jwtSettings.GoogleFrontendloginRedirectUrl;
            });

            //  تهيئة الـ Authentication وتمرير المتغيرات بشكل مباشر لمنع الـ Options التلقائية من الانهيار
            var authenticationBuilder = builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie("ExternalCookie")
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtToken))
                };
            });

            if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
            {
                authenticationBuilder.AddGoogle(options =>
                {
                    options.ClientId = googleClientId;
                    options.ClientSecret = googleClientSecret;
                    options.SignInScheme = "ExternalCookie";
                });
            }

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // الـ Middleware الخاص بالأخطاء في أول الـ Pipeline
            app.UseMiddleware<Middleware.ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI();

            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            // تطبيق الـ Migrations تلقائياً
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    if (context.Database.IsRelational())
                    {
                        await context.Database.MigrateAsync();
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "حدث خطأ أثناء تطبيق الـ Migrations على قاعدة البيانات.");
                }
            }

            app.UsePaymentWebhookVerification();
            app.MapControllers();
            await app.RunAsync();
        }
    }
}
