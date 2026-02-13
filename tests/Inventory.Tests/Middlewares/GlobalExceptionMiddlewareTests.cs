using FluentValidation;
using FluentValidation.Results;
using Inventory.Api.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace Inventory.Tests.Middlewares
{
    public class GlobalExceptionMiddlewareValidationTests
    {
        private async Task<string> GetResponseBodyAsync(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Response.Body);
            return await reader.ReadToEndAsync();
        }

        [Fact]
        public async Task InvokeAsync_Should_Return_400_When_ValidationException_Is_Thrown()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var loggerMock = new Mock<ILogger<GlobalExceptionMiddleware>>();
            var failures = new[]
            {
                new ValidationFailure("Name", "'Name' must not be empty."),
                new ValidationFailure("CategoryId", "'Category Id' must not be empty.")
            };
            var validationException = new ValidationException(failures);

            RequestDelegate next = (ctx) => throw validationException;
            var middleware = new GlobalExceptionMiddleware(next, loggerMock.Object);
            await middleware.InvokeAsync(context);
            Assert.Equal(400, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);
            var body = await GetResponseBodyAsync(context);
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement.GetProperty("error");
            Assert.Equal("Validation failed.", root.GetProperty("message").GetString());
            Assert.Equal("VALIDATION_ERROR", root.GetProperty("code").GetString());
            Assert.True(root.TryGetProperty("timestamp", out _));
            var details = root.GetProperty("details").EnumerateArray()
                .Select(d => new
                {
                    field = d.GetProperty("field").GetString(),
                    error = d.GetProperty("error").GetString()
                })
                .ToList();
            foreach (var failure in failures)
            {
                Assert.Contains(details, d => d.field == failure.PropertyName && d.error == failure.ErrorMessage);
            }
        }
    }
}