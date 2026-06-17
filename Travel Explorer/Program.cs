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

                // 🌐 تعديل الـ CORS لتسريع جلب البيانات وعمل كاش للـ Preflight Requests
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.WithOrigins("AllowAll") // تحديد دومين الـ Frontend بدقة
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .WithExposedHeaders("X-Pagination")
                              .SetPreflightMaxAge(TimeSpan.FromHours(1)); // كاش لطلب الاستئذان لمدة ساعة كاملة
                    });
                });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptions =>
                    {
                        // زيادة مهلة الانتظار لـ 60 ثانية ليعطي فرصة للـ Cold Start
                        npgsqlOptions.CommandTimeout(60); 
                        
                        // تفعيل إستراتيجية إعادة المحاولة في حال الفشل المؤقت
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 50,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorCodesToAdd:null);
                    }));


            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (jwtSettings == null)
                throw new Exception("jwt seeting is null");

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


                if (!string.IsNullOrWhiteSpace(jwtSettings.GoogleClientId) &&
                    !string.IsNullOrWhiteSpace(jwtSettings.GoogleClientSecret))
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
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>(); // استبدله باسم الـ DbContext الخاص بك
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
        
