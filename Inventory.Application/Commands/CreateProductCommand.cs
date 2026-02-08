namespace Inventory.Application.Commands;

public sealed class CreateProductCommand
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public Guid CategoryId { get; init; }
}