using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace Inventory.Api.Swagger
{
    public class SwaggerDefaultResponses : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses == null)
            {
                operation.Responses = new OpenApiResponses();
            }

            if (!operation.Responses.ContainsKey("400"))
            {
                operation.Responses.Add("400", new OpenApiResponse
                {
                    Description = "Bad Request",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Example = OpenApiAnyFactory.CreateFromJson(JsonSerializer.Serialize(new ErrorResponse
                            {
                                Error = new ErrorDetail
                                {
                                    Message = "Validation failed.",
                                    Code = "VALIDATION_ERROR",
                                    Timestamp = DateTime.UtcNow
                                }
                            }))
                        }
                    }
                });
            }

            if (!operation.Responses.ContainsKey("404"))
            {
                operation.Responses.Add("404", new OpenApiResponse
                {
                    Description = "Not Found",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Example = OpenApiAnyFactory.CreateFromJson(JsonSerializer.Serialize(new ErrorResponse
                            {
                                Error = new ErrorDetail
                                {
                                    Message = "Resource not found.",
                                    Code = "NOT_FOUND",
                                    Timestamp = DateTime.UtcNow
                                }
                            }))
                        }
                    }
                });
            }

            if (!operation.Responses.ContainsKey("500"))
            {
                operation.Responses.Add("500", new OpenApiResponse
                {
                    Description = "Internal Server Error",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Example = OpenApiAnyFactory.CreateFromJson(JsonSerializer.Serialize(new ErrorResponse
                            {
                                Error = new ErrorDetail
                                {
                                    Message = "An unexpected error occurred.",
                                    Code = "INTERNAL_SERVER_ERROR",
                                    Timestamp = DateTime.UtcNow
                                }
                            }))
                        }
                    }
                });
            }
        }
    }
}