using System.Net;
using System.Text.Json;
using Travel_Explorer.Application.Common.Exceptions;

namespace Travel_Explorer.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;
        private readonly IHostEnvironment _env = env;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            string result;

            
            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                BadRequestException => HttpStatusCode.BadRequest,
                ConflictException => HttpStatusCode.Conflict,
                ForbiddenAccessException => HttpStatusCode.Forbidden,

                
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,

                Travel_Explorer.Application.Common.Exceptions.ValidationException => HttpStatusCode.BadRequest,
                FluentValidation.ValidationException => HttpStatusCode.BadRequest,

                _ => HttpStatusCode.InternalServerError
            };

            
            switch (exception)
            {
                case Travel_Explorer.Application.Common.Exceptions.ValidationException customEx:
                    result = SerializeValidationErrors(statusCode, customEx.Errors, context);
                    break;

                case FluentValidation.ValidationException fluentEx:
                    var errors = fluentEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    result = SerializeValidationErrors(statusCode, errors, context);
                    break;

                default:
                    result = SerializeProblem(statusCode, exception, context);
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(result);
        }

        private string SerializeProblem(HttpStatusCode statusCode, Exception exception, HttpContext context)
        {
            var problemDetails = new
            {
                Status = (int)statusCode,
                Type = exception.GetType().Name,
                Title = exception.Message,
                Detail = _env.IsDevelopment() ? exception.StackTrace : null,
                Instance = context.Request.Path.Value
            };
            return JsonSerializer.Serialize(problemDetails);
        }

        private string SerializeValidationErrors(HttpStatusCode statusCode, IDictionary<string, string[]> errors, HttpContext context)
        {
            var problemDetails = new
            {
                Status = (int)statusCode,
                Type = "ValidationFailure",
                Title = "One or more validation errors occurred.",
                Instance = context.Request.Path.Value,
                Errors = errors
            };
            return JsonSerializer.Serialize(problemDetails);
        }
    }
}