using Inventory.Domain.Entities;

namespace Inventory.Application.Commands
{
    public sealed class RegisterInventoryMovementCommand
    {
        public Guid ProductId { get; init; }
        public int Quantity { get; init; }
        public InventoryMovementType Type { get; init; }
    }
}