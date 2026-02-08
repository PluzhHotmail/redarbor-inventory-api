namespace Inventory.Domain.Entities;

public sealed class InventoryMovement
{
    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private InventoryMovement()
    {
    }

    public InventoryMovement(Guid id, Guid productId, int quantity)
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        CreatedAt = DateTime.UtcNow;
    }
}