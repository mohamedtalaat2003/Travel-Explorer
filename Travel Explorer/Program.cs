using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
            builder.Services.AddSwaggerGen(
                options =>
                { 
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Travel Explorer API", Version = "v1" }),

            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
            }),

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
                }
                );



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

            // 🔥 حل المشكلة الجذري: إجبار الدوت نت على استخدام كلاس مخصص لبناء خيارات الـ JwtBearer وتخطي الـ OptionsFactory المكسور على Azure
            builder.Services.AddSingleton<Microsoft.Extensions.Options.IOptionsFactory<JwtBearerOptions>, CustomJwtBearerOptionsFactory>();

            // ✅ جلب الـ Connection String بكل الصيغ المتاحة لتأمين الاتصال بقاعدة بيانات Neon
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
      

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.CommandTimeout(10);
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null);
                    }));


            builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>(
        name: "database_health_check",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "ready" });

            // ✅ قراءة بيانات الـ JWT الأساسية صراحة لمنع أي تعامل عشوائي من السيرفر
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();


            if (!string.IsNullOrWhiteSpace(jwtSettings.GoogleClientId) &&
                !string.IsNullOrWhiteSpace(jwtSettings.GoogleClientSecret))
            {
                authenticationBuilder.AddGoogle(options =>
                builder.Services.Configure<JwtSettings>(options =>
                {
                    options.GoogleClientId = jwtSettings.GoogleClientId;
                    options.GoogleClientSecret = jwtSettings.GoogleClientSecret;
                    options.SignInScheme = "ExternalCookie";
                    options.Token = jwtSettings.Token;
                    options.Issuer = jwtSettings.Issuer;
                    options.Audience = jwtSettings.Audience;
                    options.AccessTokenExpirationMinutes = jwtSettings.AccessTokenExpirationMinutes == 0 ? 60 : jwtSettings.AccessTokenExpirationMinutes;
                    options.RefreshTokenExpirationDays = jwtSettings.RefreshTokenExpirationDays == 0 ? 7 : jwtSettings.RefreshTokenExpirationDays;
                    options.GoogleClientId = jwtSettings.GoogleClientId;
                    options.GoogleClientSecret = jwtSettings.GoogleClientSecret;
                    options.GoogleFrontendRedirectURl = jwtSettings.GoogleFrontendRedirectURl;
                    options.GoogleFrontendloginRedirectUrl = jwtSettings.GoogleFrontendloginRedirectUrl;
                });
            }

       


            // ✅ بناء الـ Authentication بشكل صارم ومباشر بدون إتاحة أي فرصة للانهيار
            var authenticationBuilder = builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie("ExternalCookie", options => {
                options.Cookie.Name = "ExternalCookie";
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtToken))
                };
            });

            if (!string.IsNullOrWhiteSpace(jwtSettings.GoogleClientId) && !string.IsNullOrWhiteSpace(jwtSettings.GoogleClientSecret))
            {
                authenticationBuilder.AddGoogle(options =>
                {
                    options.ClientId = jwtSettings.GoogleClientId;
                    options.ClientSecret = jwtSettings.GoogleClientSecret;
                    options.SignInScheme = "ExternalCookie";
                });
            }

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // الـ Middleware المخصص لمعالجة الأخطاء في مقدمة الـ Pipeline
            app.UseMiddleware<Middleware.ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI();

            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
                //using var scope = app.Services.CreateScope();
                //var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //await context.Database.MigrateAsync();


            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHealthChecks("/health");
            // تطبيق الـ Migrations تلقائياً عند التشغيل
            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var context = services.GetRequiredService<ApplicationDbContext>();
            //        if (context.Database.IsRelational())
            //        {
            //            await context.Database.MigrateAsync();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "حدث خطأ أثناء تطبيق الـ Migrations على قاعدة البيانات.");
            //    }
            //}

            app.UsePaymentWebhookVerification();
            app.MapControllers();
            await app.RunAsync();
        }
    }

    // =====================================================================================
    // ✅ كلاس مخصص لتخطي ميكانيكية الـ OptionsFactory الافتراضية التالفة على سيرفر Azure
    // =====================================================================================
    public class CustomJwtBearerOptionsFactory : Microsoft.Extensions.Options.IOptionsFactory<JwtBearerOptions>
    {
        private readonly IConfiguration _configuration;
        public CustomJwtBearerOptionsFactory(IConfiguration configuration) => _configuration = configuration;

        public JwtBearerOptions Create(string name)
        {
            var token = _configuration["JwtSettings:Token"] ?? _configuration["JwtSettings__Token"];
            var issuer = _configuration["JwtSettings:Issuer"] ?? _configuration["JwtSettings__Issuer"];
            var audience = _configuration["JwtSettings:Audience"] ?? _configuration["JwtSettings__Audience"];

            var options = new JwtBearerOptions();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token ?? "FallbackTokenAtLeast32BytesLongShouldBeReplaced!"))
            };
            return options;
        }
    }
}
