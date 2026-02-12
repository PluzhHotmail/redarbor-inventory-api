using System.Net;
using System.Text.Json;
using FluentValidation;
using Inventory.Application.Exceptions;

namespace Inventory.Api.Middlewares
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Capturamos la excepción raíz
                var baseException = ex.GetBaseException();
                _logger.LogError(baseException, "Unhandled exception occurred.");

                await HandleExceptionAsync(context, baseException);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            object response;
            int statusCode;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = new
                    {
                        error = new
                        {
                            message = "Validation failed.",
                            code = "VALIDATION_ERROR",
                            timestamp = DateTime.UtcNow,
                            details = validationException.Errors.Select(e => new
                            {
                                field = e.PropertyName,
                                error = e.ErrorMessage
                            })
                        }
                    };
                    break;

                case NotFoundException notFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    response = new
                    {
                        error = new
                        {
                            message = notFoundException.Message,
                            code = "NOT_FOUND",
                            timestamp = DateTime.UtcNow
                        }
                    };
                    break;

                case InvalidOperationException invalidOperationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = new
                    {
                        error = new
                        {
                            message = invalidOperationException.Message,
                            code = "INVALID_OPERATION",
                            timestamp = DateTime.UtcNow
                        }
                    };
                    break;

                default:
                    // Siempre devolvemos un mensaje genérico si no es un tipo conocido
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    response = new
                    {
                        error = new
                        {
                            message = exception.Message, // aquí podemos mostrar el mensaje real para debugging
                            code = "INTERNAL_SERVER_ERROR",
                            timestamp = DateTime.UtcNow
                        }
                    };
                    break;
            }

            context.Response.StatusCode = statusCode;
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}