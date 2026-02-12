namespace Inventory.Api.Swagger
{
    public class ErrorDetail
    {
        public string Message { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}