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
                    policy.AllowAnyOrigin() // تم تعديلها لتسمح بأي أصل مؤقتاً لحل مشاكل الاتصال، يمكنك تخصيصها لاحقاً
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("X-Pagination")
                          .SetPreflightMaxAge(TimeSpan.FromHours(1));
                });
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? builder.Configuration["ConnectionStrings__DefaultConnection"],
                    npgsqlOptions =>
                    {
                        // زيادة مهلة الانتظار لـ 60 ثانية ليعطي فرصة للـ Cold Start
                        npgsqlOptions.CommandTimeout(60); 
                        
                        // تفعيل إستراتيجية إعادة المحاولة في حال الفشل المؤقت
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorCodesToAdd: null);
                    }));

            //  الطريقة الآمنة والمباشرة لقراءة إعدادات الـ JWT لتفادي خطأ الـ Null والمشاكل السحابية
            var jwtSettings = new JwtSettings
            {
                Token = builder.Configuration["JwtSettings:Token"] ?? builder.Configuration["JwtSettings__Token"],
                Issuer = builder.Configuration["JwtSettings:Issuer"] ?? builder.Configuration["JwtSettings__Issuer"],
                Audience = builder.Configuration["JwtSettings:Audience"] ?? builder.Configuration["JwtSettings__Audience"],
                AccessTokenExpirationMinutes = int.TryParse(builder.Configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? builder.Configuration["JwtSettings__AccessTokenExpirationMinutes"], out var accessMins) ? accessMins : 60,
                RefreshTokenExpirationDays = int.TryParse(builder.Configuration["JwtSettings:RefreshTokenExpirationDays"] ?? builder.Configuration["JwtSettings__RefreshTokenExpirationDays"], out var refreshDays) ? refreshDays : 7,
                GoogleClientId = builder.Configuration["JwtSettings:GoogleClientId"] ?? builder.Configuration["JwtSettings__GoogleClientId"],
                GoogleClientSecret = builder.Configuration["JwtSettings:GoogleClientSecret"] ?? builder.Configuration["JwtSettings__GoogleClientSecret"],
                
                // هنا تم دمج الاحتمالين (Uri و Url) لضمان القراءة الصحيحة من اللوكال ومن Azure
                GoogleFrontendRedirectURl = builder.Configuration["JwtSettings:GoogleFrontendRedirectUri"] ?? builder.Configuration["JwtSettings__GoogleFrontendRedirectUri"] ?? builder.Configuration["JwtSettings:GoogleFrontendRedirectUrl"] ?? builder.Configuration["JwtSettings__GoogleFrontendRedirectUrl"],
                GoogleFrontendloginRedirectUrl = builder.Configuration["JwtSettings:GoogleFrontendloginRedirectUrl"] ?? builder.Configuration["JwtSettings__GoogleFrontendloginRedirectUrl"]
            };

            // حماية للتطبيق: إذا لم يجد السيرفر التوكن الأساسي، يخرج بتقرير واضح بدلاً من الانهيار المبهم
            if (string.IsNullOrEmpty(jwtSettings.Token))
            {
                throw new Exception("Critical Error: 'JwtSettings:Token' or 'JwtSettings__Token' is missing from configuration settings!");
            }

            // تسجيل الإعدادات في نظام الـ Options
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

            var authenticationBuilder = builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie("ExternalCookie")
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

            // الـ Middleware الخاص بمعالجة الأخطاء يجب أن يكون أول شيء
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

            // تطبيق الـ Migrations تلقائياً عند التشغيل
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
