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

            // Translate Azure dash-separated configuration keys and environment variables to nested keys
            var translatedConfig = new Dictionary<string, string>();
            foreach (var item in builder.Configuration.AsEnumerable())
            {
                var key = item.Key;
                var val = item.Value;
                if (!string.IsNullOrEmpty(val))
                {
                    if (key.StartsWith("APPSETTING_", StringComparison.OrdinalIgnoreCase))
                    {
                        var cleanKey = key.Substring("APPSETTING_".Length);
                        if (cleanKey.Contains('-'))
                        {
                            translatedConfig[cleanKey.Replace('-', ':')] = val;
                        }
                    }
                    else if (key.Contains('-'))
                    {
                        translatedConfig[key.Replace('-', ':')] = val;
                    }
                }
            }
            if (translatedConfig.Count > 0)
            {
                builder.Configuration.AddInMemoryCollection(translatedConfig!);
            }


            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Travel Explorer API", Version = "v1" });


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


            // 🌐 تعديل الـ CORS لتسريع جلب البيانات وعمل كاش للـ Preflight Requests
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("https://travel-explorer-jade.vercel.app") // تحديد دومين الـ Frontend بدقة
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("X-Pagination")
                          .SetPreflightMaxAge(TimeSpan.FromHours(1)); // كاش لطلب الاستئذان لمدة ساعة كاملة
                });
            });


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

            builder.Services.Configure<JwtSettings>(options =>
            {
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

            builder.Services.Configure<PaymobtSettings>(builder.Configuration.GetSection("PaymobSettings"));

            var authenticationBuilder = builder.Services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddCookie("ExternalCookie")
                .AddJwtBearer(options =>
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Token))
                    };
                });




            //if (!string.IsNullOrWhiteSpace(jwtSettings.GoogleClientId) &&
            //    !string.IsNullOrWhiteSpace(jwtSettings.GoogleClientSecret))
            //{
            //    authenticationBuilder.AddGoogle(options =>
            //    {
            //        options.ClientId = jwtSettings.GoogleClientId;
            //        options.ClientSecret = jwtSettings.GoogleClientSecret;
            //        options.SignInScheme = "ExternalCookie";
            //    });
            //}

            builder.Services.AddAuthorization();

            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var configuration = services.GetRequiredService<IConfiguration>();
                    var connString = configuration.GetConnectionString("DefaultConnection");
                    if (string.IsNullOrWhiteSpace(connString))
                    {
                        connString = configuration["POSTGRESQLCONNSTR_DefaultConnection"] ??
                                     Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_DefaultConnection");
                    }

                    if (string.IsNullOrWhiteSpace(connString))
                    {
                        Console.WriteLine("Warning: DefaultConnection connection string is missing or empty. Database migrations and seeding skipped.");
                        Console.WriteLine("Diagnostics (Startup): Scanning available configuration keys that might contain connection settings...");
                        try
                        {
                            foreach (var child in configuration.AsEnumerable())
                            {
                                if (child.Key.Contains("Conn", StringComparison.OrdinalIgnoreCase) ||
                                    child.Key.Contains("Postgres", StringComparison.OrdinalIgnoreCase) ||
                                    child.Key.Contains("Default", StringComparison.OrdinalIgnoreCase) ||
                                    child.Key.Contains("Db", StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine($"Config Key found: '{child.Key}' (Length: {child.Value?.Length ?? 0})");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Diagnostics (Startup): Failed to scan configuration keys: {ex.Message}");
                        }
                    }
                    else
                    {
                        var db = services.GetRequiredService<ApplicationDbContext>();
                        await db.Database.MigrateAsync();

                        await RoleSeeder.SeedRolesAsync(services.GetRequiredService<RoleManager<IdentityRole<int>>>());
                        await AdminSeeder.SeedAsync(services);
                        await DataSeeder.SeedAsync(services);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during database migration or seeding: {ex.Message}");
                }
            }

            app.UseMiddleware<Middleware.ExceptionMiddleware>();


            app.UseSwagger();
            app.UseSwaggerUI();

            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            // تفعيل سياسة الـ CORS قبل الـ Authentication والـ Authorization
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UsePaymentWebhookVerification();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}