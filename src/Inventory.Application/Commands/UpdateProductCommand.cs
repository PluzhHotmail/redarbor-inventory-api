namespace Inventory.Application.Commands;

public sealed class UpdateProductCommand
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Stock { get; init; } = 0;
    public Guid CategoryId { get; init; } = Guid.Empty;
}