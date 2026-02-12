using FluentValidation;
using Inventory.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Inventory.Api.Middlewares
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionMiddleware> logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var errorCode = "INTERNAL_SERVER_ERROR";
            var message = "An unexpected error occurred.";

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = "VALIDATION_ERROR";
                    message = string.Join(" | ", validationException.Errors.Select(e => e.ErrorMessage));
                    break;

                case InvalidOperationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = "INVALID_OPERATION";
                    message = exception.Message;
                    break;

                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorCode = "NOT_FOUND";
                    message = exception.Message;
                    break;
            }

            var response = new
            {
                error = new
                {
                    message,
                    code = errorCode,
                    timestamp = DateTime.UtcNow
                }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}