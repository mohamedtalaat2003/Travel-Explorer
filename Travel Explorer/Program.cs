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
                        policy.WithOrigins("https://travel-explorer-jade.vercel.app") // تحديد دومين الـ Frontend بدقة
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .WithExposedHeaders("X-Pagination")
                              .SetPreflightMaxAge(TimeSpan.FromHours(1)); // كاش لطلب الاستئذان لمدة ساعة كاملة
                    });
                });


                var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();

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

                app.UsePaymentWebhookVerification();

                app.MapControllers();

                await app.RunAsync();
            }
        }
    }
        