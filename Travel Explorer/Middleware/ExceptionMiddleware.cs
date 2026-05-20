using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
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
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError;
            string result;

            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    result = SerializeProblem(statusCode, exception, context);
                    break;

                case ForbiddenAccessException:
                    statusCode = HttpStatusCode.Forbidden;
                    result = SerializeProblem(statusCode, exception, context);
                    break;

                case BadRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = SerializeProblem(statusCode, exception, context);
                    break;

                case Travel_Explorer.Application.Common.Exceptions.ValidationException customEx:
                    statusCode = HttpStatusCode.BadRequest;
                    result = SerializeValidationErrors(statusCode, customEx.Errors, context);
                    break;

                case FluentValidation.ValidationException fluentEx:
                    statusCode = HttpStatusCode.BadRequest;
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
            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Type = exception.GetType().Name,
                Title = exception.Message,
                Detail = _env.IsDevelopment() ? exception.StackTrace : null,
                Instance = context.Request.Path
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
