using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Travel_Explorer.Application.Common.Exceptions;


namespace Travel_Explorer.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case ForbiddenAccessException:
                    statusCode = HttpStatusCode.Forbidden;
                    break;
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                default:
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            if (result == string.Empty)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = (int)statusCode,
                    Type = exception.GetType().Name,
                    Title = exception.Message,
                    Detail = _env.IsDevelopment() ? exception.StackTrace : null,
                    Instance = context.Request.Path
                };

                result = JsonSerializer.Serialize(problemDetails);
            }

            await context.Response.WriteAsync(result);
        }
    }
}
